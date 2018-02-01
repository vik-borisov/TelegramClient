namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Threading.Tasks;

    internal interface ITcpTransport : IDisposable
    {
        Task<byte[]> Receieve();

        Task Disconnect();
        Task Send(byte[] packet);
    }
}