using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TelegramClient.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TelegramClient.Core
{
    using TelegramClient.Core.ApiServies.Interfaces;

    public interface ITelegramClient
    {
        ISenderService SendService { get; }

        IUpdatesApiService UpdatesService { get; }

        IConnectApiService ConnectService { get; }

        IAuthApiService AuthService { get; }

        IMessagesApiService MessagesService { get; }

        IContactsApiService ContactsService { get; }

        IUploadApiService UploadService { get; }
    }
}