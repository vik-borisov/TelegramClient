namespace TelegramClient.Core.Network
{
    using System;
    using System.Threading.Tasks;

    internal interface ITcpTransport: IDisposable
    {
        Task<TcpMessage> SendAndReceieve(byte[] packet);

        void Send(byte[] packet);
    }
}