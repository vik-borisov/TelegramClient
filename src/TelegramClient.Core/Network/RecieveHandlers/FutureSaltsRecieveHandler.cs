namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using log4net;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class FutureSaltsRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FutureSaltsRecieveHandler));

        public uint ResponceCode { get; } = 0xae500895;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            var requestId = reader.ReadUInt64();

            Log.Debug($"Handle Future Salts for request {requestId}");

            throw new NotImplementedException("The future sault does not supported yet");
        }
    }
}