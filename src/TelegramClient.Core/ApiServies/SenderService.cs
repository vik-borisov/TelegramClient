namespace TelegramClient.Core.ApiServies
{
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

        public IResponseResultGetter ResponseResultGetter { get; set; }

        public async Task<TResult> SendRequestAsync<TResult>(IRequest<TResult> methodToExecute)
        {
            Log.Debug($"Send message of the constructor {methodToExecute}");

            object result;
            try
            {
                result = await SendAndRecieve(methodToExecute).ConfigureAwait(false);
            }
            catch (BadServerSaltException)
            {
                result = await SendAndRecieve(methodToExecute).ConfigureAwait(false);
            }

            return (TResult)result;
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