namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IConfirmationRecieveService))]
    internal class ConfirmationRecieveService : IConfirmationRecieveService
    {
        private readonly ConcurrentDictionary<ulong, TaskCompletionSource<bool>> _waitConfirm = new ConcurrentDictionary<ulong, TaskCompletionSource<bool>>();

        public Task WaitForConfirm(ulong messageId)
        {
            var tsc = new TaskCompletionSource<bool>();
            _waitConfirm.TryAdd(messageId, tsc);

            return tsc.Task;
        }

        public void ConfirmRequest(ulong requestId)
        {
            if (_waitConfirm.TryGetValue(requestId, out var tsc))
            {
                tsc.SetResult(true);
            }
        }

        public void RequestWithException(ulong requestId, Exception exception)
        {
            if (_waitConfirm.TryGetValue(requestId, out var tsc))
            {
                tsc.SetException(exception);
            }
        }
    }
}