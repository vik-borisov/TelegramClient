namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class BadServerSaltRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BadServerSaltRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(TBadServerSalt) };

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public void HandleResponce(IObject obj)
        {
            var message = obj.Cast<TBadServerSalt>();
            
            Log.Info($"Bad server sault detected! message id = {message.BadMsgId} ");

            ClientSettings.Session.Salt = message.NewServerSalt;

            ConfirmationRecieveService.RequestWithException(message.BadMsgId, new BadServerSaltException());
        }
    }
}