namespace TelegramClient.Core
{
    using System;

    using Castle.Windsor;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(ITelegramClient))]
    internal class Client : ITelegramClient
    {
        public ISenderService SendService { get; set; }

        public IUpdatesApiService UpdatesService { get; set; }

        public IConnectApiService ConnectService { get; set; }

        public IAuthApiService AuthService { get; set; }

        public IMessagesApiService MessagesService { get; set; }

        public IContactsApiService ContactsService { get; set; }

        public IUploadApiService UploadService { get; set; }

        internal IWindsorContainer Container { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Container?.Dispose();
            }
        }

        ~Client()
        {
            Dispose(false);
        }
    }
}