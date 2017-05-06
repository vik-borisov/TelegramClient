namespace TelegramClient.Core.Network.Tcp
{
    using System.IO;
    using System.Threading.Tasks;

    internal interface ITcpService
    {
        Task Send(byte[] encodedMessage);

        Task<Stream> Receieve();
    }
}