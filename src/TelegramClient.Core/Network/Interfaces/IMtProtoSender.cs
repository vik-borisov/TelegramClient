namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using OpenTl.Schema;

    internal interface IMtProtoSender
    { 
        Task<(Task, long)> SendWithConfim(IObject obj, CancellationToken cancellationToken);

        Task<long> SendWithoutConfirm(IObject obj, CancellationToken cancellationToken);
    }
}