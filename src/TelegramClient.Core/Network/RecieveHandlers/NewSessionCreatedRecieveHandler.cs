namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class NewSessionCreatedRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewSessionCreatedRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(TNewSessionCreated) };

        public void HandleResponce(IObject obj)
        {
            Log.Debug("Handle a new session was created");
        }
    }
}