namespace TelegramClient.Core.Network
{
    using System.Net.Sockets;
    using System.Threading.Tasks;

    internal interface ITcpService
    {
        Task Send(byte[] encodedMessage);

        Task<NetworkStream> Receieve();
    }
}