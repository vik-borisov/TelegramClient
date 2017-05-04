namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading.Tasks;

    using TelegramClient.Entities;

    internal interface IMtProtoSender
    {
        Task Send(TlMethod request);
    }
}