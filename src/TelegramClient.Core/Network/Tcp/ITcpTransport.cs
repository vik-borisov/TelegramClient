namespace TelegramClient.Core.Network.Tcp
{
    using System.Threading.Tasks;

    internal interface ITcpTransport
    {
        Task Send(byte[] packet);

        Task<byte[]> Receieve();
    }
}