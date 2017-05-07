namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class MsgsAckRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MsgsAckRecieveHandler));

        public uint[] HandleCodes { get; } = { 0x62d6b459 };

        public byte[] HandleResponce(uint code, BinaryReader reader)
        {
            Log.Debug("Handle a messages ack");

            return null;
        }
    }
}