namespace TelegramClient.Core.Network
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.IO.Compression;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Interfaces;

    internal class MtProtoRecieveService : IMtProtoReciever, IMtProtoRecieveService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoRecieveService));

        private readonly ConcurrentDictionary<ulong, TaskCompletionSource<BinaryReader>> _resultCallbacks = new ConcurrentDictionary<ulong, TaskCompletionSource<BinaryReader>>();

        public ITcpTransport TcpTransport { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public IConfirmationSendService ConfirmationSendService { get; set; }

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public void StartReceiving()
        {
            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    while (true)
                    {
                        try
                        {
                            var recieveTask = TcpTransport.Receieve();
                            recieveTask.Wait();
                            var result = recieveTask.Result;
                            ProcessReceivedMessage(result);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Recieve message failed", e);
                        }
                    }
                });
        }

        public Task<BinaryReader> Recieve(ulong requestId)
        {
            var tcs = new TaskCompletionSource<BinaryReader>();

            _resultCallbacks[requestId] = tcs;
            return tcs.Task;
        }

        private void ProcessReceivedMessage(TcpMessage message)
        {
            var result = DecodeMessage(message.Body);

            Log.Debug($"Recieve message with remote id: {result.Item2}");

            using (var messageStream = new MemoryStream(result.Item1, false))
            using (var messageReader = new BinaryReader(messageStream))
            {
                ProcessMessage(result.Item2, result.Item3, messageReader);
            }

            ConfirmationSendService.AddForSend(result.Item2);
        }

        private void ReturnResult(ulong requestId, byte[] reader)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                var stream = new MemoryStream(reader);
                var binaryReader = new BinaryReader(stream);
                callback.SetResult(binaryReader);
            }
            else
            {
                Log.Debug($"Request with Id {requestId} wasn't not handled");
            }
        }

        private void ReturnException(ulong requestId, Exception exception)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                callback.SetException(exception);
            }
            else
            {
                Log.Debug($"Request with Id {requestId} wasn't not handled");
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
                var keyData = TlHelpers.CalcKey(ClientSettings.Session.AuthKey.Data, msgKey, false);

                var plaintext = AES.DecryptAes(keyData,
                    inputReader.ReadBytes((int)(inputStream.Length - inputStream.Position)));

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

        private bool ProcessMessage(ulong messageId, int sequence, BinaryReader messageReader)
        {
            // TODO: check salt
            // TODO: check sessionid
            // TODO: check seqno

            //logger.debug("processMessage: msg_id {0}, sequence {1}, data {2}", BitConverter.ToString(((MemoryStream)messageReader.BaseStream).GetBuffer(), (int) messageReader.BaseStream.Position, (int) (messageReader.BaseStream.Length - messageReader.BaseStream.Position)).Replace("-","").ToLower());

            var code = messageReader.ReadUInt32();
            messageReader.BaseStream.Position -= 4;
            switch (code)
            {
                case 0x73f1f8dc: // container

                    //logger.debug("MSG container");
                    return HandleContainer(messageId,  sequence, messageReader);
                case 0x7abe77ec: // ping

                    //logger.debug("MSG ping");
                    return HandlePing();
                case 0x347773c5: // pong

                    //logger.debug("MSG pong");
                    return HandlePong(messageReader);
                case 0xae500895: // future_salts

                    //logger.debug("MSG future_salts");
                    return HandleFutureSalts(messageReader);
                case 0x9ec20908: // new_session_created

                    //logger.debug("MSG new_session_created");
                    return HandleNewSessionCreated();
                case 0x62d6b459: // msgs_ack

                    //logger.debug("MSG msds_ack");
                    return HandleMsgsAck();
                case 0xedab447b: // bad_server_salt

                    //logger.debug("MSG bad_server_salt");
                    return HandleBadServerSalt(messageReader);
                case 0xa7eff811: // bad_msg_notification

                    //logger.debug("MSG bad_msg_notification");
                    return HandleBadMsgNotification(messageReader);
                case 0x276d3ec6: // msg_detailed_info

                    //logger.debug("MSG msg_detailed_info");
                    return HandleMsgDetailedInfo();
                case 0xf35c6d01: // rpc_result

                    //logger.debug("MSG rpc_result");
                    return HandleRpcResult(messageId, messageReader);
                case 0x3072cfa1: // gzip_packed

                    //logger.debug("MSG gzip_packed");
                    return HandleGzipPacked(messageId, sequence, messageReader);
                case 0xe317af7e:
                case 0xd3f45784:
                case 0x2b2fbd4e:
                case 0x78d4dec1:
                case 0x725b04c3:
                case 0x74ae4240:
                    return HandleUpdate(messageId);
                default:
                    Log.Error($"Recieved unknown message code: {code}");
                    return false;
            }
        }

        private bool HandleUpdate(ulong messageId)
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

        private bool HandleGzipPacked(ulong messageId, int sequence, BinaryReader messageReader)
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
                    ProcessMessage(messageId, sequence, compressedReader);
                }
            }


            return true;
        }

        private bool HandleRpcResult(ulong messageId, BinaryReader messageReader)
        {
            var code = messageReader.ReadUInt32();
            var requestId = messageReader.ReadUInt64();

            Log.Debug($"Process RpcResult  with request id = '{requestId}' and messageId = '{messageId}'");

            ConfirmationRecieveService.ConfirmRequest(requestId);

            //if (requestId == (ulong)request.MessageId)
            //{
            //    Log.Debug($"Request id = '{requestId}' is confirmed");
            //    request.ConfirmReceived = true;
            //}

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

                Exception exception;
                if (errorMessage.StartsWith("FLOOD_WAIT_"))
                {
                    var resultString = Regex.Match(errorMessage, @"\d+").Value;
                    var seconds = int.Parse(resultString);
                    exception = new FloodException(TimeSpan.FromSeconds(seconds));
                }
                else if (errorMessage.StartsWith("PHONE_MIGRATE_"))
                {
                    var resultString = Regex.Match(errorMessage, @"\d+").Value;
                    var dcIdx = int.Parse(resultString);
                    exception = new PhoneMigrationException(dcIdx);
                }
                else if (errorMessage.StartsWith("FILE_MIGRATE_"))
                {
                    var resultString = Regex.Match(errorMessage, @"\d+").Value;
                    var dcIdx = int.Parse(resultString);
                    exception = new FileMigrationException(dcIdx);
                }
                else if (errorMessage.StartsWith("USER_MIGRATE_"))
                {
                    var resultString = Regex.Match(errorMessage, @"\d+").Value;
                    var dcIdx = int.Parse(resultString);
                    exception = new UserMigrationException(dcIdx);
                }
                else if (errorMessage == "PHONE_CODE_INVALID")
                    exception = new InvalidPhoneCodeException("The numeric code used to authenticate does not match the numeric code sent by SMS/Telegram");
                else if (errorMessage == "SESSION_PASSWORD_NEEDED")
                    exception = new CloudPasswordNeededException("This Account has Cloud Password !");
                else
                    exception = new InvalidOperationException(errorMessage);

                ReturnException(requestId, exception);
                return true;
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

                        ReturnResult(requestId, ms.ToArray());
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
                var bytes = messageReader.ReadBytes((int)(messageReader.BaseStream.Length - messageReader.BaseStream.Position));

                ReturnResult(requestId, bytes);
            }

            return false;
        }

        private bool HandleMsgDetailedInfo()
        {
            return false;
        }

        private bool HandleBadMsgNotification(BinaryReader messageReader)
        {
            var code = messageReader.ReadUInt32();
            var requestId = messageReader.ReadUInt64();
            var requestSequence = messageReader.ReadInt32();
            var errorCode = messageReader.ReadInt32();

            Exception exception = null;
            switch (errorCode)
            {
                case 16:
                    exception = new InvalidOperationException(
                        "msg_id too low (most likely, client time is wrong; it would be worthwhile to synchronize it using msg_id notifications and re-send the original message with the “correct” msg_id or wrap it in a container with a new msg_id if the original message had waited too long on the client to be transmitted)");
                    break;
                case 17:
                    exception = new InvalidOperationException(
                        "msg_id too high (similar to the previous case, the client time has to be synchronized, and the message re-sent with the correct msg_id)");
                    break;
                case 18:
                     exception = new InvalidOperationException(
                        "incorrect two lower order msg_id bits (the server expects client message msg_id to be divisible by 4)");
                    break;
                case 19:
                     exception = new InvalidOperationException(
                        "container msg_id is the same as msg_id of a previously received message (this must never happen)");
                    break;
                case 20:
                     exception = new InvalidOperationException(
                        "message too old, and it cannot be verified whether the server has received a message with this msg_id or not");
                    break;
                case 32:
                     exception = new InvalidOperationException(
                        "msg_seqno too low (the server has already received a message with a lower msg_id but with either a higher or an equal and odd seqno)");
                    break;
                case 33:
                     exception = new InvalidOperationException(
                        " msg_seqno too high (similarly, there is a message with a higher msg_id but with either a lower or an equal and odd seqno)");
                    break;
                case 34:
                     exception = new InvalidOperationException(
                        "an even msg_seqno expected (irrelevant message), but odd received");
                    break;
                case 35:
                     exception = new InvalidOperationException("odd msg_seqno expected (relevant message), but even received");
                    break;
                case 48:
                     exception = new InvalidOperationException(
                        "incorrect server salt (in this case, the bad_server_salt response is received with the correct salt, and the message is to be re-sent with it)");
                    break;
                case 64:
                     exception = new InvalidOperationException("invalid container");
                    break;
                default:
                    exception = new NotImplementedException("This should never happens");
                    break;
            }

            ConfirmationRecieveService.RequestWithException(requestId, exception);

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

            return true;
        }

        private bool HandleBadServerSalt(BinaryReader messageReader)
        {
            var code = messageReader.ReadUInt32();
            var badMsgId = messageReader.ReadUInt64();
            var badMsgSeqNo = messageReader.ReadInt32();
            var errorCode = messageReader.ReadInt32();
            var newSalt = messageReader.ReadUInt64();

            Log.Info($"Bad server sault detected! message id = {badMsgId} ");

            //logger.debug("bad_server_salt: msgid {0}, seq {1}, errorcode {2}, newsalt {3}", badMsgId, badMsgSeqNo, errorCode, newSalt);

            ClientSettings.Session.Salt = newSalt;

            ConfirmationRecieveService.RequestWithException(badMsgId, new BadServerSaltException());

            //resend
            //Log.Debug($"Retry resend the request '{request}' with a valid server sault");
            //var preparedData = PrepareToSend(tcpTransport, request);
            //TcpTransport.Send(preparedData);
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

        private bool HandleMsgsAck()
        {
            return false;
        }

        private bool HandleNewSessionCreated()
        {
            return false;
        }

        private bool HandleFutureSalts(BinaryReader messageReader)
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

        private bool HandlePong(BinaryReader messageReader)
        {
            var code = messageReader.ReadUInt32();
            var requestId = messageReader.ReadUInt64();

            ConfirmationRecieveService.ConfirmRequest(requestId);

            return false;
        }

        private bool HandlePing()
        {
            return false;
        }

        private bool HandleContainer(ulong messageId, int sequence, BinaryReader messageReader)
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

                    var messageProcessed = ProcessMessage(innerMessageId, sequence, messageReader);
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
    }
}