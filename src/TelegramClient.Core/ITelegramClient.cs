using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TelegramClient.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TelegramClient.Core
{
    using System.Threading.Tasks;

    using Autofac;

    using OpenTl.Schema;

    using TelegramClient.Core.ApiServies;


    public interface ITelegramClient
    {
        IComponentContext Container { get;}

        Task ConnectAsync(bool reconnect = false);

        Task<TResult> SendRequestAsync<TResult>(IRequest<TResult> requestToExecute);

        Task<IUpdates> SendMessageAsync(IInputPeer peer, string message);

        Task ReconnectToDcAsync(int dcId);

        IUpdatesApiService Updates { get; set; }
    }
}