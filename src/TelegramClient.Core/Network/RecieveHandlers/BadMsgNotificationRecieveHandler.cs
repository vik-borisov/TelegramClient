namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class BadMsgNotificationRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BadMsgNotificationRecieveHandler));

        public uint ResponceCode { get; } = 0xa7eff811;

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle a bad message notification");

            var code = reader.ReadUInt32();
            var requestId = reader.ReadUInt64();
            var requestSequence = reader.ReadInt32();
            var errorCode = reader.ReadInt32();

            Exception exception;
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

            return Enumerable.Empty<byte[]>();
        }
    }
}