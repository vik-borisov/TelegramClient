using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TelegramClient.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TelegramClient.Core
{
    using System.Threading.Tasks;

    using Autofac;

    using TelegramClient.Core.ApiServies;
    using TelegramClient.Entities;
    using TelegramClient.Entities.TL;


    public interface ITelegramClient
    {
        IComponentContext Container { get;}

        Task ConnectAsync(bool reconnect = false);

        Task<T> SendRequestAsync<T>(TlMethod methodToExecute);

        Task<TlAbsUpdates> SendMessageAsync(TlAbsInputPeer peer, string message);

        Task ReconnectToDcAsync(int dcId);

        IUpdatesApiService Updates { get; set; }
    }
}