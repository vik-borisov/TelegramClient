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

        public int[] HandleCodes { get; } = { -1631450872 };

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            Log.Debug("Handle a new session was created");

            return null;
        }
    }
}