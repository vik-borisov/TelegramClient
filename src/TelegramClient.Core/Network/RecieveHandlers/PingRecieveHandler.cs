namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class PingRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PingRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(RequestPing) };

        public void HandleResponce(IObject obj)
        {
            Log.Debug("Handle ping");
        }
    }
}