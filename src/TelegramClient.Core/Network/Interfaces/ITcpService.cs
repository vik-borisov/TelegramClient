namespace TelegramClient.Core.Network.Interfaces
{
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    internal interface ITcpService
    {
        Task Send(byte[] encodedMessage);

        Task<Stream> Receieve();
    }
}