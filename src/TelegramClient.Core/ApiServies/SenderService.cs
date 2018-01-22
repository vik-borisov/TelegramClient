namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Threading.Tasks;

    using Castle.DynamicProxy.Generators.Emitters;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(ISenderService))]
    internal class SenderService : ISenderService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SenderService));

        public IMtProtoSender Sender { get; set; }

        public IResponseResultGetter ResponseResultGetter { get; set; }

        public Lazy<IConnectApiService> ConnectApiService { get; set; }
        
        public async Task<TResult> SendRequestAsync<TResult>(IRequest<TResult> methodToExecute)
        {
            Log.Debug($"Send message of the constructor {methodToExecute}");

            while (true)
            {
                try
                {
                    return (TResult) await SendAndRecieve(methodToExecute).ConfigureAwait(false);
                }
                catch (BadServerSaltException)
                {
                    return (TResult) await SendAndRecieve(methodToExecute).ConfigureAwait(false);
                }
                catch (AuthRestartException)
                {
                    await ConnectApiService.Value.ReAuthenticateAsync().ConfigureAwait(false);
                }
                catch (PhoneMigrationException ex)
                {
                    await ConnectApiService.Value.ReconnectToDcAsync(ex.Dc).ConfigureAwait(false);
                }
            }
        }

        private async Task<object> SendAndRecieve(IObject methodToExecute)
        {
            var sendTask = await Sender.Send(methodToExecute).ConfigureAwait(false);
            var recieveTask = ResponseResultGetter.Recieve(sendTask.Item2);

            await sendTask.Item1.ConfigureAwait(false);
            await recieveTask.ConfigureAwait(false);

            return recieveTask.Result;
        }
    }
}