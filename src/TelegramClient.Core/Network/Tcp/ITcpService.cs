namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal interface ITcpService : IDisposable
    {
        Task Disconnect();

        Task<int> Read(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

        Task Send(byte[] encodedMessage, CancellationToken cancellationToken);
    }
}