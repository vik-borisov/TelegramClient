namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Updates;

    public delegate Task UpdateHandler(IUpdates update);

    public interface IUpdatesApiService
    {
        Task<IState> GetCurrentState(CancellationToken cancellationToken = default(CancellationToken));

        Task<IDifference> GetUpdates(IState currentState, CancellationToken cancellationToken = default(CancellationToken));

        event UpdateHandler RecieveUpdates;
    }

    internal interface IUpdatesApiServiceRaiser
    {
        Task OnUpdateRecieve(IUpdates message);
    }
}