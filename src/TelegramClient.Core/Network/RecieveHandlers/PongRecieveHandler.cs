namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class PongRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PongRecieveHandler));

        public int[] HandleCodes { get; } = { 0x347773c5 };

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            var requestId = reader.ReadUInt64();

            Log.Debug($"Handle pong for request = {requestId}");

            ConfirmationRecieveService.ConfirmRequest(requestId);

            return null;
        }
    }
}