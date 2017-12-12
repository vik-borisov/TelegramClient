namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using Newtonsoft.Json;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class MsgsAckRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MsgsAckRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(TMsgsAck) };

        public void HandleResponce(IObject obj)
        {
            Log.Debug("Handle a messages ack");

            if (Log.IsDebugEnabled)
            {
                var message = obj.Cast<TMsgsAck>();

                var jMessages = JsonConvert.SerializeObject(message.MsgIds.Items);
                Log.Debug($"Receiving confirmation of the messages: {jMessages}");
            }
        }
    }
}