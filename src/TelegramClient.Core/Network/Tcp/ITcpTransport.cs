namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Threading.Tasks;

    internal interface ITcpTransport: IDisposable
    {
        Task Send(byte[] packet);

        Task<byte[]> Receieve();
    }
}