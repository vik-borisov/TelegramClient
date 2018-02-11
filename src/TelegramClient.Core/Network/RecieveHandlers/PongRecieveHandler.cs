namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using Newtonsoft.Json;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class PongRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PongRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(TPong) };

        public void HandleResponce(IObject obj)
        {
            var message = obj.Cast<TPong>();

            if (Log.IsDebugEnabled)
            {
                var jMessages = JsonConvert.SerializeObject(message);
                Log.Debug($"Handle pong for request = {jMessages}");
            }
        }
    }
}