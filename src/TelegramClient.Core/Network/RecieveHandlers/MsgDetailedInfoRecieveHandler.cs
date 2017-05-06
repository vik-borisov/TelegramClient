namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class MsgDetailedInfoRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PingRecieveHandler));

        public uint ResponceCode { get; } = 0x276d3ec6;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle MsgDetailedInfo");

            return Enumerable.Empty<byte[]>();
        }
    }
}