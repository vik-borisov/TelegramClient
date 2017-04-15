namespace TelegramClient.Core.Network
{
    using System.Threading.Tasks;

    using TelegramClient.Entities;

    internal interface IMtProtoSender
    {
        Task<byte[]> SendAndRecive(TlMethod request);

        Task SendPingAsync();
    }
}