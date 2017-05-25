namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class MsgsAckRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MsgsAckRecieveHandler));

        public int[] HandleCodes { get; } = { 0x62d6b459 };

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            Log.Debug("Handle a messages ack");

            var vector = reader.ReadInt32();
            var count = reader.ReadInt32();

            var ackMessages = new List<ulong>();
            for (var i = 0; i < count; i++)
            {
                ackMessages.Add(reader.ReadUInt64());
            }

            Log.Debug($"Receiving confirmation of the messages: {string.Join(",", ackMessages)}");

            return null;
        }
    }
}