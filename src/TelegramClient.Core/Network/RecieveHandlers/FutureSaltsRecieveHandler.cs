namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class FutureSaltsRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FutureSaltsRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(TFutureSalts) };

        public void HandleResponce(IObject obj)
        {
            var message = obj.Cast<TFutureSalts>();

            Log.Debug($"Handle Future Salts for request {message.ReqMsgId}");

            throw new NotImplementedException("The future sault does not supported yet");
        }
    }
}