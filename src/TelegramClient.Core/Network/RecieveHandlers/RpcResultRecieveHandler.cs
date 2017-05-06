namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text.RegularExpressions;

    using log4net;

    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class RpcResultRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RpcResultRecieveHandler));

        public uint ResponceCode { get; } = 0xf35c6d01;

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IResponseResultSetter ResponseResultSetter { get; set; }

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle RpcResult");

            var requestId = reader.ReadUInt64();

            Log.Debug($"Process RpcResult  with request id = '{requestId}'");

            ConfirmationRecieveService.ConfirmRequest(requestId);

            var innerCode = reader.ReadUInt32();
            switch (innerCode)
            {
                case 0x2144ca19:
                    HandleRpcError(reader, requestId);
                    break;
                case 0x3072cfa1:
                    HandleZipPacket(reader, requestId);
                    break;
                default:
                    reader.BaseStream.Position -= 4;
                    var bytes = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

                    ResponseResultSetter.ReturnResult(requestId, bytes);
                    break;
            }

            return Enumerable.Empty<byte[]>();
        }

        private void HandleZipPacket(BinaryReader reader, ulong requestId)
        {
            try
            {
                // gzip_packed
                var packedData = Serializers.Bytes.Read(reader);

                using (var ms = new MemoryStream())
                {
                    using (var packedStream = new MemoryStream(packedData, false))
                    using (var zipStream = new GZipStream(packedStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(ms);
                        ms.Position = 0;
                    }

                    ResponseResultSetter.ReturnResult(requestId, ms.ToArray());
                }
            }
            catch (NotSupportedException ex)
            {
                Log.Error("gzip_packed", ex);
            }
        }

        private void HandleRpcError(BinaryReader reader, ulong requestId)
        {
            // rpc_error
            var errorCode = reader.ReadInt32();
            var errorMessage = Serializers.String.Read(reader);

            Log.Warn($"Recieve error from server: {errorMessage}");

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

            ResponseResultSetter.ReturnException(requestId, exception);
        }
    }
}