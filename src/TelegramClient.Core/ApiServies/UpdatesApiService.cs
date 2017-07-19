namespace TelegramClient.Core.ApiServies
{
    using OpenTl.Schema;

    using TelegramClient.Core.IoC;

    public delegate void UpdateHandler(IUpdates update);

    public interface IUpdatesApiService
    {
        event UpdateHandler RecieveUpdates;
    }

    internal interface IUpdatesApiServiceRaiser
    {
        void OnUpdateRecieve(IUpdates message);
    }

    [SingleInstance(typeof(IUpdatesApiService), typeof(IUpdatesApiServiceRaiser))]
    internal class UpdatesApiService : IUpdatesApiService,
                                       IUpdatesApiServiceRaiser
    {
        public event UpdateHandler RecieveUpdates;

        public void OnUpdateRecieve(IUpdates message)
        {
            RecieveUpdates?.Invoke(message);
        }
    }
}