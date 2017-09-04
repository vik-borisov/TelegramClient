namespace TelegramClient.Core.ApiServies
{
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Updates;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IUpdatesApiService), typeof(IUpdatesApiServiceRaiser))]
    internal class UpdatesApiService : IUpdatesApiService,
                                       IUpdatesApiServiceRaiser
    {
        public ISenderService SenderService { get; set; }

        public event UpdateHandler RecieveUpdates;

        public void OnUpdateRecieve(IUpdates message)
        {
            RecieveUpdates?.Invoke(message);
        }

        public async Task<IState> GetCurrentState()
        {
            return await SenderService.SendRequestAsync(new RequestGetState());
        }

        public async Task<IDifference> GetUpdates(IState currentState)
        {
            var getDiffRequest = new RequestGetDifference
                                 {
                                     Pts = currentState.Pts,
                                     Qts = currentState.Qts,
                                     Date = currentState.Date
                                 };

            return await SenderService.SendRequestAsync(getDiffRequest);
        }
    }
}