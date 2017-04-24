﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelegramClient.Core.MTProto;
using TelegramClient.Core.MTProto.Crypto;
using TelegramClient.Core.Requests;
using TelegramClient.Core.Utils;
using TelegramClient.Entities;

namespace TelegramClient.Core.Network
{
    using log4net;

    using TelegramClient.Core.Exceptions;
	using TelegramClient.Core.Settings;

	internal class MtProtoSender : IMtProtoSender
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoSender));

        private readonly HashSet<ulong> _needConfirmation = new HashSet<ulong>();

		public ITcpTransport TcpTransport { get; set; }

		public IClientSettings ClientSettings { get; set; }


        private byte[] PrepareToSend(ITcpTransport tcpTransport, TlMethod request)
        {
            // TODO: refactor
            if (_needConfirmation.Any())
            {
                Log.Debug($"Sending confirmation for messages {string.Join(",", _needConfirmation.Select(m => m.ToString()))}");

                var ackRequest = new AckRequest(_needConfirmation);
                using (var memory = new MemoryStream())
                using (var writer = new BinaryWriter(memory))
                {
                    ackRequest.SerializeBody(writer);
                    var confirmedPacket = PrepareToSend(memory.ToArray(), ackRequest);
                    tcpTransport.Send(confirmedPacket);

                    _needConfirmation.Clear();
                }
            }

            using (var memory = new MemoryStream())
            using (var writer = new BinaryWriter(memory))
            {
                request.SerializeBody(writer);
                return PrepareToSend(memory.ToArray(), request);
            }
        }

        private byte[] PrepareToSend(byte[] packet, TlMethod request)
		{
			request.MessageId = ClientSettings.Session.GetNewMessageId();

            Log.Debug($"Send message with Id = {request.MessageId}");

			byte[] msgKey;
			byte[] ciphertext;
			using (var plaintextPacket = MakeMemory(8 + 8 + 8 + 4 + 4 + packet.Length))
			{
				using (var plaintextWriter = new BinaryWriter(plaintextPacket))
				{
					plaintextWriter.Write(ClientSettings.Session.Salt);
					plaintextWriter.Write(ClientSettings.Session.Id);
					plaintextWriter.Write(request.MessageId);
					plaintextWriter.Write(ClientSettings.Session.GenerateSessionSeqNo(request.Confirmed));
					plaintextWriter.Write(packet.Length);
					plaintextWriter.Write(packet);

					plaintextPacket.TryGetBuffer(out var buffer);
					msgKey = Helpers.CalcMsgKey(buffer.Array);
					ciphertext = AES.EncryptAes(Helpers.CalcKey(ClientSettings.Session.AuthKey.Data, msgKey, true), buffer.Array);
				}
			}

			using (var ciphertextPacket = MakeMemory(8 + 16 + ciphertext.Length))
			{
				using (var writer = new BinaryWriter(ciphertextPacket))
				{
					writer.Write(ClientSettings.Session.AuthKey.Id);
					writer.Write(msgKey);
					writer.Write(ciphertext);

					return ciphertextPacket.ToArray();
				}
			}
		}

		private Tuple<byte[], ulong, int> DecodeMessage(byte[] body)
		{
			byte[] message;
			ulong remoteMessageId;
			int remoteSequence;

			using (var inputStream = new MemoryStream(body))
			using (var inputReader = new BinaryReader(inputStream))
			{
				if (inputReader.BaseStream.Length < 8)
					throw new InvalidOperationException($"Can't decode packet");

				var remoteAuthKeyId = inputReader.ReadUInt64(); // TODO: check auth key id
				var msgKey = inputReader.ReadBytes(16); // TODO: check msg_key correctness
				var keyData = Helpers.CalcKey(ClientSettings.Session.AuthKey.Data, msgKey, false);

				var plaintext = AES.DecryptAes(keyData,
					inputReader.ReadBytes((int) (inputStream.Length - inputStream.Position)));

				using (var plaintextStream = new MemoryStream(plaintext))
				using (var plaintextReader = new BinaryReader(plaintextStream))
				{
					var remoteSalt = plaintextReader.ReadUInt64();
					var remoteSessionId = plaintextReader.ReadUInt64();
					remoteMessageId = plaintextReader.ReadUInt64();
					remoteSequence = plaintextReader.ReadInt32();
					var msgLen = plaintextReader.ReadInt32();
					message = plaintextReader.ReadBytes(msgLen);
				}
			}

			return new Tuple<byte[], ulong, int>(message, remoteMessageId, remoteSequence);
		}

	    private void ProcessReceivedMessage(ITcpTransport tcpTransport, TcpMessage message, TlMethod request)
	    {
	        while (!request.ConfirmReceived)
	        {
	            var result = DecodeMessage(message.Body);

	            Log.Debug($"Recieve message with remote id: {result.Item2}");

	            using (var messageStream = new MemoryStream(result.Item1, false))
	            using (var messageReader = new BinaryReader(messageStream))
	            {
	                ProcessMessage(tcpTransport, result.Item2, result.Item3, messageReader, request);
	            }
	        }
	    }

	    public async Task SendAndProcess(TlMethod request)
	    {
	        var preparedData = PrepareToSend(TcpTransport, request);
	        var result = await TcpTransport.SendAndReceieve(preparedData);

	        ProcessReceivedMessage(TcpTransport, result, request);
	    }

	    public async Task SendPingAsync()
	    {
	        var pingRequest = new PingRequest();
	        using (var memory = new MemoryStream())
	        using (var writer = new BinaryWriter(memory))
	        {
	            pingRequest.SerializeBody(writer);
	            var preparedData =  PrepareToSend(memory.ToArray(), pingRequest);
	            var result = await TcpTransport.SendAndReceieve(preparedData);
	            ProcessReceivedMessage(TcpTransport, result, pingRequest);
            }
        }

	    private bool ProcessMessage(ITcpTransport tcpTransport, ulong messageId, int sequence, BinaryReader messageReader, TlMethod request)
	    {
	        // TODO: check salt
	        // TODO: check sessionid
	        // TODO: check seqno

	        //logger.debug("processMessage: msg_id {0}, sequence {1}, data {2}", BitConverter.ToString(((MemoryStream)messageReader.BaseStream).GetBuffer(), (int) messageReader.BaseStream.Position, (int) (messageReader.BaseStream.Length - messageReader.BaseStream.Position)).Replace("-","").ToLower());
	        _needConfirmation.Add(messageId);

	        var code = messageReader.ReadUInt32();
	        messageReader.BaseStream.Position -= 4;
	        switch (code)
	        {
	            case 0x73f1f8dc: // container

	                //logger.debug("MSG container");
	                return HandleContainer(tcpTransport, messageId, sequence, messageReader, request);
	            case 0x7abe77ec: // ping

	                //logger.debug("MSG ping");
	                return HandlePing(messageId, sequence, messageReader);
	            case 0x347773c5: // pong

	                //logger.debug("MSG pong");
	                return HandlePong(messageId, sequence, messageReader, request);
	            case 0xae500895: // future_salts

	                //logger.debug("MSG future_salts");
	                return HandleFutureSalts(messageId, sequence, messageReader);
	            case 0x9ec20908: // new_session_created

	                //logger.debug("MSG new_session_created");
	                return HandleNewSessionCreated(messageId, sequence, messageReader);
	            case 0x62d6b459: // msgs_ack

	                //logger.debug("MSG msds_ack");
	                return HandleMsgsAck(messageId, sequence, messageReader);
	            case 0xedab447b: // bad_server_salt

	                //logger.debug("MSG bad_server_salt");
	                return HandleBadServerSalt(tcpTransport, messageId, sequence, messageReader, request);
	            case 0xa7eff811: // bad_msg_notification

	                //logger.debug("MSG bad_msg_notification");
	                return HandleBadMsgNotification(messageId, sequence, messageReader);
	            case 0x276d3ec6: // msg_detailed_info

	                //logger.debug("MSG msg_detailed_info");
	                return HandleMsgDetailedInfo(messageId, sequence, messageReader);
	            case 0xf35c6d01: // rpc_result

	                //logger.debug("MSG rpc_result");
	                return HandleRpcResult(messageId, sequence, messageReader, request);
	            case 0x3072cfa1: // gzip_packed

	                //logger.debug("MSG gzip_packed");
	                return HandleGzipPacked(tcpTransport, messageId, sequence, messageReader, request);
	            case 0xe317af7e:
	            case 0xd3f45784:
	            case 0x2b2fbd4e:
	            case 0x78d4dec1:
	            case 0x725b04c3:
	            case 0x74ae4240:
	                return HandleUpdate(messageId, sequence, messageReader);
	            default:
	                Log.Error($"Recieved unknown message code: {code}");
	                return false;
	        }
	    }

	    private bool HandleUpdate(ulong messageId, int sequence, BinaryReader messageReader)
		{
		    Log.Debug($"Recieved update message with id = {messageId}");

            return false;

			/*
			try
			{
				UpdatesEvent(TL.Parse<Updates>(messageReader));
				return true;
			}
			catch (Exception e)
			{
				logger.warning("update processing exception: {0}", e);
				return false;
			}
			*/
		}

		private bool HandleGzipPacked(ITcpTransport tcpTransport, ulong messageId, int sequence, BinaryReader messageReader, TlMethod request)
		{
            Log.Debug($"Recived Gzip message with Id = {messageId}");

			var code = messageReader.ReadUInt32();
			using (var decompressStream = new MemoryStream())
			{
				using (var stream = new MemoryStream(Serializers.Bytes.Read(messageReader)))
				using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
				{
					zipStream.CopyTo(decompressStream);
			 
				}

			    decompressStream.Position = 0;
				using (var compressedReader = new BinaryReader(decompressStream))
				{
                    ProcessMessage(tcpTransport, messageId, sequence, compressedReader, request);
				}
			}


			return true;
		}

		private bool HandleRpcResult(ulong messageId, int sequence, BinaryReader messageReader, TlMethod request)
		{
			var code = messageReader.ReadUInt32();
			var requestId = messageReader.ReadUInt64();

		    Log.Debug($"Process RpcResult  with request id = '{requestId}' and messageId = '{messageId}'");

            if (requestId == (ulong)request.MessageId)
		    {
		        Log.Debug($"Request id = '{requestId}' is confirmed");
                request.ConfirmReceived = true;
            }

            //throw new NotImplementedException();
            /*
			lock (runningRequests)
			{
				if (!runningRequests.ContainsKey(requestId))
				{
					logger.warning("rpc response on unknown request: {0}", requestId);
					messageReader.BaseStream.Position -= 12;
					return false;
				}

				request = runningRequests[requestId];
				runningRequests.Remove(requestId);
			}
			*/

            var innerCode = messageReader.ReadUInt32();
			if (innerCode == 0x2144ca19)
			{
				// rpc_error
				var errorCode = messageReader.ReadInt32();
				var errorMessage = Serializers.String.Read(messageReader);

                Log.Info($"Recieve error from server: {errorMessage}");

				if (errorMessage.StartsWith("FLOOD_WAIT_"))
				{
					var resultString = Regex.Match(errorMessage, @"\d+").Value;
					var seconds = int.Parse(resultString);
					throw new FloodException(TimeSpan.FromSeconds(seconds));
				}
				if (errorMessage.StartsWith("PHONE_MIGRATE_"))
				{
					var resultString = Regex.Match(errorMessage, @"\d+").Value;
					var dcIdx = int.Parse(resultString);
					throw new PhoneMigrationException(dcIdx);
				}
				if (errorMessage.StartsWith("FILE_MIGRATE_"))
				{
					var resultString = Regex.Match(errorMessage, @"\d+").Value;
					var dcIdx = int.Parse(resultString);
					throw new FileMigrationException(dcIdx);
				}
				if (errorMessage.StartsWith("USER_MIGRATE_"))
				{
					var resultString = Regex.Match(errorMessage, @"\d+").Value;
					var dcIdx = int.Parse(resultString);
					throw new UserMigrationException(dcIdx);
				}
				if (errorMessage == "PHONE_CODE_INVALID")
					throw new InvalidPhoneCodeException(
						"The numeric code used to authenticate does not match the numeric code sent by SMS/Telegram");
				if (errorMessage == "SESSION_PASSWORD_NEEDED")
					throw new CloudPasswordNeededException("This Account has Cloud Password !");
				throw new InvalidOperationException(errorMessage);
			}
			if (innerCode == 0x3072cfa1)
			{
				try
				{
					// gzip_packed
					var packedData = Serializers.Bytes.Read(messageReader);
					using (var ms = new MemoryStream())
					{
						using (var packedStream = new MemoryStream(packedData, false))
						using (var zipStream = new GZipStream(packedStream, CompressionMode.Decompress))
						{
							zipStream.CopyTo(ms);
							ms.Position = 0;
						}
						using (var compressedReader = new BinaryReader(ms))
						{
							request.DeserializeResponse(compressedReader);
                        }
                    }

				}
				catch (NotSupportedException ex)
				{
                    Log.Error("gzip_packed", ex);
				}
			}
			else
			{
				messageReader.BaseStream.Position -= 4;
				request.DeserializeResponse(messageReader);
            }

            return false;
		}

		private bool HandleMsgDetailedInfo(ulong messageId, int sequence, BinaryReader messageReader)
		{
			return false;
		}

		private bool HandleBadMsgNotification(ulong messageId, int sequence, BinaryReader messageReader)
		{
			var code = messageReader.ReadUInt32();
			var requestId = messageReader.ReadUInt64();
			var requestSequence = messageReader.ReadInt32();
			var errorCode = messageReader.ReadInt32();

			switch (errorCode)
			{
				case 16:
					throw new InvalidOperationException(
						"msg_id too low (most likely, client time is wrong; it would be worthwhile to synchronize it using msg_id notifications and re-send the original message with the “correct” msg_id or wrap it in a container with a new msg_id if the original message had waited too long on the client to be transmitted)");
				case 17:
					throw new InvalidOperationException(
						"msg_id too high (similar to the previous case, the client time has to be synchronized, and the message re-sent with the correct msg_id)");
				case 18:
					throw new InvalidOperationException(
						"incorrect two lower order msg_id bits (the server expects client message msg_id to be divisible by 4)");
				case 19:
					throw new InvalidOperationException(
						"container msg_id is the same as msg_id of a previously received message (this must never happen)");
				case 20:
					throw new InvalidOperationException(
						"message too old, and it cannot be verified whether the server has received a message with this msg_id or not");
				case 32:
					throw new InvalidOperationException(
						"msg_seqno too low (the server has already received a message with a lower msg_id but with either a higher or an equal and odd seqno)");
				case 33:
					throw new InvalidOperationException(
						" msg_seqno too high (similarly, there is a message with a higher msg_id but with either a lower or an equal and odd seqno)");
				case 34:
					throw new InvalidOperationException(
						"an even msg_seqno expected (irrelevant message), but odd received");
				case 35:
					throw new InvalidOperationException("odd msg_seqno expected (relevant message), but even received");
				case 48:
					throw new InvalidOperationException(
						"incorrect server salt (in this case, the bad_server_salt response is received with the correct salt, and the message is to be re-sent with it)");
				case 64:
					throw new InvalidOperationException("invalid container");
			}
			throw new NotImplementedException("This should never happens");
			/*
			logger.debug("bad_msg_notification: msgid {0}, seq {1}, errorcode {2}", requestId, requestSequence,
						 errorCode);
			*/
			/*
			if (!runningRequests.ContainsKey(requestId))
			{
				logger.debug("bad msg notification on unknown request");
				return true;
			}
			*/

			//OnBrokenSessionEvent();
			//MTProtoRequest request = runningRequests[requestId];
			//request.OnException(new MTProtoBadMessageException(errorCode));
		}

        private bool HandleBadServerSalt(ITcpTransport tcpTransport, ulong messageId, int sequence, BinaryReader messageReader, TlMethod request)
        {
            var code = messageReader.ReadUInt32();
            var badMsgId = messageReader.ReadUInt64();
            var badMsgSeqNo = messageReader.ReadInt32();
            var errorCode = messageReader.ReadInt32();
            var newSalt = messageReader.ReadUInt64();

            Log.Info($"Bad server sault detected! message id = {badMsgId} ");

            //logger.debug("bad_server_salt: msgid {0}, seq {1}, errorcode {2}, newsalt {3}", badMsgId, badMsgSeqNo, errorCode, newSalt);

            ClientSettings.Session.Salt = newSalt;

            //resend
            Log.Debug($"Retry resend the request '{request}' with a valid server sault");
            var preparedData = PrepareToSend(tcpTransport, request);
            TcpTransport.Send(preparedData);
            /*
			if(!runningRequests.ContainsKey(badMsgId)) {
				logger.debug("bad server salt on unknown message");
				return true;
			}
			*/


            //MTProtoRequest request = runningRequests[badMsgId];
            //request.OnException(new MTProtoBadServerSaltException(salt));

            return true;
        }

        private bool HandleMsgsAck(ulong messageId, int sequence, BinaryReader messageReader)
		{
			return false;
		}

		private bool HandleNewSessionCreated(ulong messageId, int sequence, BinaryReader messageReader)
		{
			return false;
		}

		private bool HandleFutureSalts(ulong messageId, int sequence, BinaryReader messageReader)
		{
			var code = messageReader.ReadUInt32();
			var requestId = messageReader.ReadUInt64();

			messageReader.BaseStream.Position -= 12;

			throw new NotImplementedException("Handle future server salts function isn't implemented.");
			/*
			if (!runningRequests.ContainsKey(requestId))
			{
				logger.info("future salts on unknown request");
				return false;
			}
			*/

			//	MTProtoRequest request = runningRequests[requestId];
			//	runningRequests.Remove(requestId);
			//	request.OnResponse(messageReader);

			return true;
		}

		private bool HandlePong(ulong messageId, int sequence, BinaryReader messageReader, TlMethod request)
		{
			var code = messageReader.ReadUInt32();
			var msgId = messageReader.ReadUInt64();

			if (msgId == (ulong) request.MessageId)
				request.ConfirmReceived = true;

			return false;
		}

		private bool HandlePing(ulong messageId, int sequence, BinaryReader messageReader)
		{
			return false;
		}

		private bool HandleContainer(ITcpTransport transport, ulong messageId, int sequence, BinaryReader messageReader, TlMethod request)
		{
            var code = messageReader.ReadUInt32();
			var size = messageReader.ReadInt32();
			for (var i = 0; i < size; i++)
			{
				var innerMessageId = messageReader.ReadUInt64();
				var innerSequence = messageReader.ReadInt32();
				var innerLength = messageReader.ReadInt32();
				var beginPosition = messageReader.BaseStream.Position;
				try
				{
                    Log.Debug($"Recieve container with id = '{messageId}' and innerId = '{innerMessageId}'");

				    var messageProcessed = ProcessMessage(transport, innerMessageId, sequence, messageReader, request);
				    if (!messageProcessed)
                    {
                        messageReader.BaseStream.Position = beginPosition + innerLength;
                    }
                }
				catch (Exception e)
				{
				    Log.Error("Failed to process message in contailer", e);

                    //	logger.error("failed to process message in contailer: {0}", e);
                    messageReader.BaseStream.Position = beginPosition + innerLength;
				}
			}

			return false;
		}

		private MemoryStream MakeMemory(int len)
		{
			return new MemoryStream(new byte[len], 0, len, true, true);
		}
	}

	public class FloodException : Exception
	{
		internal FloodException(TimeSpan timeToWait)
			: base(
				$"Flood prevention. Telegram now requires your program to do requests again only after {timeToWait.TotalSeconds} seconds have passed ({nameof(TimeToWait)} property)." +
				" If you think the culprit of this problem may lie in TLSharp's implementation, open a Github issue please.")
		{
			TimeToWait = timeToWait;
		}

		public TimeSpan TimeToWait { get; }
	}

	internal abstract class DataCenterMigrationException : Exception
	{
		private const string ReportMessage =
			" See: https://github.com/sochix/TLSharp#i-get-a-xxxmigrationexception-or-a-migrate_x-error";

		protected DataCenterMigrationException(string msg, int dc) : base(msg + ReportMessage)
		{
			Dc = dc;
		}

		internal int Dc { get; }
	}

	internal class PhoneMigrationException : DataCenterMigrationException
	{
		internal PhoneMigrationException(int dc)
			: base($"Phone number registered to a different DC: {dc}.", dc)
		{
		}
	}

	internal class FileMigrationException : DataCenterMigrationException
	{
		internal FileMigrationException(int dc)
			: base($"File located on a different DC: {dc}.", dc)
		{
		}
	}

	internal class UserMigrationException : DataCenterMigrationException
	{
		internal UserMigrationException(int dc)
			: base($"User located on a different DC: {dc}.", dc)
		{
		}
	}
}