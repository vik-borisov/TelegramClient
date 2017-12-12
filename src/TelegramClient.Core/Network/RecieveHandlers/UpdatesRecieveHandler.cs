namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using Newtonsoft.Json;

    using OpenTl.Schema;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class UpdatesRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UpdatesRecieveHandler));

        public Type[] HandleCodes { get; } =
            {
                typeof(TUpdateShortMessage),
                typeof(TUpdateShortSentMessage),
                typeof(TUpdatesTooLong),
                typeof(TUpdateShortChatMessage),
                typeof(TUpdateShort),
                typeof(TUpdatesCombined),
                typeof(TUpdates)
            };

        public IUpdatesApiServiceRaiser UpdateRaiser { get; set; }

        public void HandleResponce(IObject obj)
        {
            if (Log.IsDebugEnabled)
            {
                var jUpdate = JsonConvert.SerializeObject(obj);
                Log.Debug($"Recieve Updates \n{jUpdate}");
            }

            UpdateRaiser.OnUpdateRecieve(obj.Cast<IUpdates>());
        }
    }
}