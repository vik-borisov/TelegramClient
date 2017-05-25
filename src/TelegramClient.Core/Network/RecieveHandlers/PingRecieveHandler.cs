namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class PingRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PingRecieveHandler));

        public int[] HandleCodes { get; } = { 0x7abe77ec };

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            Log.Debug("Handle ping");

            return null;
        }
    }
}