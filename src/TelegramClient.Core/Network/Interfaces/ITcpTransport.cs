namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading.Tasks;

    internal interface ITcpTransport
    {
        void Send(byte[] packet);

        Task<TcpMessage> Receieve();
    }
}