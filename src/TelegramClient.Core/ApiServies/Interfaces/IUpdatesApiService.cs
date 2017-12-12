namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Updates;

    public delegate void UpdateHandler(IUpdates update);

    public interface IUpdatesApiService
    {
        Task<IState> GetCurrentState();

        Task<IDifference> GetUpdates(IState currentState);

        //
        event UpdateHandler RecieveUpdates;
    }

    internal interface IUpdatesApiServiceRaiser
    {
        void OnUpdateRecieve(IUpdates message);
    }
}