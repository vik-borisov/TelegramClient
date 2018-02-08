namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading.Tasks;

    using OpenTl.Schema;

    internal interface IMtProtoSender
    {
        Task<(Task, long)> SendWithConfim(IObject obj);

        Task<long> SendWithoutConfirm(IObject obj);
    }
}