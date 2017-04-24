namespace TelegramClient.Core.Network
{
    using System.Threading.Tasks;

    using TelegramClient.Entities;

    internal interface IMtProtoSender
    {
        Task SendAndProcess(TlMethod request);

        Task SendPingAsync();
    }
}