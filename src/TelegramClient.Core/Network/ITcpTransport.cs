namespace TelegramClient.Core.Network
{
    using System.Threading.Tasks;

    internal interface ITcpTransport
    {
        Task<TcpMessage> SendAndReceieve(byte[] packet);

        void Send(byte[] packet);
    }
}