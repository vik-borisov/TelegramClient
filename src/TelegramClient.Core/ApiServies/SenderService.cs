namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(ISenderService))]
    internal class SenderService : ISenderService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SenderService));

        public IMtProtoSender Sender { get; set; }

        public Lazy<IConnectApiService> ConnectApiService { get; set; }

        public async Task<TResult> SendRequestAsync<TResult>(IRequest<TResult> methodToExecute, CancellationToken cancellationToken = default(CancellationToken))
        {
            while (true)
            {
                Log.Debug($"Send message of the constructor {methodToExecute}");
                
                try
                {
                    return (TResult)await await Sender.SendAndWaitResponse(methodToExecute, cancellationToken).ConfigureAwait(false);
                }
                catch (BadServerSaltException)
                {
                }
                catch (AuthRestartException)
                {
                    await ConnectApiService.Value.ReAuthenticateAsync();
                }
                catch (DataCenterMigrationException ex)
                {
                    await ConnectApiService.Value.ReconnectToDcAsync(ex.Dc);
                }
            }
        }
    }
}