namespace TelegramClient.Core
{
    using System;
    using System.Threading.Tasks;

    using LightInject;

    using TelegramClient.Entities;
    using TelegramClient.Entities.TL;

    public interface ITelegramClient: IDisposable
    {
        IServiceContainer Container { get;}

        Task ConnectAsync(bool reconnect = false);

        Task<T> SendRequestAsync<T>(TlMethod methodToExecute);

        Task<TlAbsUpdates> SendMessageAsync(TlAbsInputPeer peer, string message);

        Task ReconnectToDcAsync(int dcId);

        Task SendPingAsync();
    }
}