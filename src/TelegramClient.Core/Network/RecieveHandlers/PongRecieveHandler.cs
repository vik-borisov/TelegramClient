namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class PongRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PongRecieveHandler));

        public uint ResponceCode { get; } = 0x347773c5;

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            var requestId = reader.ReadUInt64();

            Log.Debug($"Handle pong for request = {requestId}");

            ConfirmationRecieveService.ConfirmRequest(requestId);

            return Enumerable.Empty<byte[]>();
        }
    }
}