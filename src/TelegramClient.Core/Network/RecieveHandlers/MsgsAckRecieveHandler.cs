namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class MsgsAckRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MsgsAckRecieveHandler));

        public uint ResponceCode { get; } = 0x62d6b459;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle a messages ack");

            return Enumerable.Empty<byte[]>();
        }
    }
}