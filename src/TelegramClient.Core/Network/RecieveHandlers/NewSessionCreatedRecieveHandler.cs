namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class NewSessionCreatedRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewSessionCreatedRecieveHandler));

        public uint[] HandleCodes { get; } = { 0x9ec20908 };

        public byte[] HandleResponce(uint code, BinaryReader reader)
        {
            Log.Debug("Handle a new session was created");

            return null;
        }
    }
}