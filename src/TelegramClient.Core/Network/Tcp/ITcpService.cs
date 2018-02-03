namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    internal interface ITcpService : IDisposable
    {
        Task<int> Read(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
        Task Disconnect();
        Task Connect();
        Task Send(byte[] encodedMessage, CancellationToken cancellationToken);
    }
}