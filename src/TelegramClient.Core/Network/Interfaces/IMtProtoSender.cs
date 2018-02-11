namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using OpenTl.Schema;

    internal interface IMtProtoSender
    { 
        Task<long> Send(IObject obj, CancellationToken cancellationToken);

        Task<Task<object>> SendAndWaitResponse(IObject obj, CancellationToken cancellationToken);
    }
}