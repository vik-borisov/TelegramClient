namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class PingRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PingRecieveHandler));

        public uint ResponceCode { get; } = 0x7abe77ec;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle ping");

            return Enumerable.Empty<byte[]>();
        }
    }
}