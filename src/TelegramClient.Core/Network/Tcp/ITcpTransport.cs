namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Threading.Tasks;

    internal interface ITcpTransport : IDisposable
    {
        Task Disconnect();

        Task<byte[]> Receieve();

        Task Send(byte[] packet);
    }
}