namespace TelegramClient.Core
{
    using Autofac;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(ITelegramClient))]
    internal class Client : ITelegramClient
    {
        internal IComponentContext Container { get; set; }

        public ISenderService SendService { get; set; }

        public IUpdatesApiService UpdatesService { get; set; }

        public IConnectApiService ConnectService { get; set; }

        public IAuthApiService AuthService { get; set; }

        public IMessagesApiService MessagesService { get; set; }

        public IContactsApiService ContactsService { get; set; }

        public IUploadApiService UploadService { get; set; }
    }
}