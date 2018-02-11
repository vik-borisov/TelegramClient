namespace TelegramClient.Core.ApiServies
{
    using System.Threading;
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

        public async Task<IState> GetCurrentState(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SenderService.SendRequestAsync(new RequestGetState(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IDifference> GetUpdates(IState currentState, CancellationToken cancellationToken = default(CancellationToken))
        {
            var getDiffRequest = new RequestGetDifference
                                 {
                                     Pts = currentState.Pts,
                                     Qts = currentState.Qts,
                                     Date = currentState.Date
                                 };

            return await SenderService.SendRequestAsync(getDiffRequest, cancellationToken).ConfigureAwait(false);
        }

        public async Task OnUpdateRecieve(IUpdates message)
        {
            if (RecieveUpdates != null)
            {
                await RecieveUpdates.Invoke(message).ConfigureAwait(false);
            }
        }

        public event UpdateHandler RecieveUpdates;
    }
}