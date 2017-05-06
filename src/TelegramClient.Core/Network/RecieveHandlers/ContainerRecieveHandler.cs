namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;

    using log4net;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class ContainerRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContainerRecieveHandler));

        public uint ResponceCode { get; } = 0x73f1f8dc;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            Log.Debug("Handle container");

            var code = reader.ReadUInt32();
            var size = reader.ReadInt32();

            for (var i = 0; i < size; i++)
            {
                var innerMessageId = reader.ReadUInt64();
                var innerSequence = reader.ReadInt32();
                var innerLength = reader.ReadInt32();

                Log.Debug($"Process responce with inner id = '{innerMessageId}' into container");

                yield return reader.ReadBytes(innerLength);
            }
        }
    }
}