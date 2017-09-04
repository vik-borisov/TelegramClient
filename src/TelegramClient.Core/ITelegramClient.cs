using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TelegramClient.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TelegramClient.Core
{
    using TelegramClient.Core.ApiServies.Interfaces;

    public interface ITelegramClient
    {
        /// <summary>Send custom messages</summary>
        ISenderService SendService { get; }

        /// <summary>Automatic and manual updates</summary>
        IUpdatesApiService UpdatesService { get; }

        /// <summary>Connecting to the server</summary>
        IConnectApiService ConnectService { get; }

        /// <summary>Registration and authentication</summary>
        IAuthApiService AuthService { get; }

        /// <summary>Messages and chats</summary>
        IMessagesApiService MessagesService { get; }

        /// <summary>Working with contacts</summary>
        IContactsApiService ContactsService { get; }

        /// <summary>Working with files</summary>
        IUploadApiService UploadService { get; }
    }
}