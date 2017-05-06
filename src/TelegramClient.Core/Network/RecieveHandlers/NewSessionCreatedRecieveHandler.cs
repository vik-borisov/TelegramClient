namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class NewSessionCreatedRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewSessionCreatedRecieveHandler));

        public uint ResponceCode { get; } = 0x9ec20908;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle a new session was created");

            return Enumerable.Empty<byte[]>();
        }
    }
}