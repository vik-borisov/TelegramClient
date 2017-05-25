namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class FutureSaltsRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FutureSaltsRecieveHandler));

        public int[] HandleCodes { get; } = { -1370486635 };

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            var requestId = reader.ReadUInt64();

            Log.Debug($"Handle Future Salts for request {requestId}");

            throw new NotImplementedException("The future sault does not supported yet");
        }
    }
}