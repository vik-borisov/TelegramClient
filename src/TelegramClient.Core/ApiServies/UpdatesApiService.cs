namespace TelegramClient.Core.ApiServies
{
    using TelegramClient.Core.IoC;
    using TelegramClient.Entities.TL;

    public delegate void UpdateHandler(TlAbsUpdates update);

    public interface IUpdatesApiService
    {
        event UpdateHandler RecieveUpdates;
    }

    internal interface IUpdatesApiServiceRaiser
    {
        void OnUpdateRecieve(TlAbsUpdates message);
    }

    [SingleInstance(typeof(IUpdatesApiService), typeof(IUpdatesApiServiceRaiser))]
    internal class UpdatesApiService : IUpdatesApiService,
                                       IUpdatesApiServiceRaiser
    {
        public event UpdateHandler RecieveUpdates;

        public void OnUpdateRecieve(TlAbsUpdates message)
        {
            RecieveUpdates?.Invoke(message);
        }
    }
}