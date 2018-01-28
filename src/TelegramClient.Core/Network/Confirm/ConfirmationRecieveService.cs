namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IConfirmationRecieveService))]
    internal class ConfirmationRecieveService : IConfirmationRecieveService
    {
        private readonly ConcurrentDictionary<long, TaskCompletionSource<bool>> _waitConfirm = new ConcurrentDictionary<long, TaskCompletionSource<bool>>();

        public void ConfirmRequest(long requestId)
        {
            if (_waitConfirm.TryGetValue(requestId, out var tsc))
            {
                tsc.SetResult(true);
                _waitConfirm.TryRemove(requestId, out var ignored);
            }
        }

        public void RequestWithException(long requestId, Exception exception)
        {
            if (_waitConfirm.TryGetValue(requestId, out var tsc))
            {
                tsc.SetException(exception);
                _waitConfirm.TryRemove(requestId, out var ignored);
            }
        }

        public Task WaitForConfirm(long messageId)
        {
            var tsc = new TaskCompletionSource<bool>();
            _waitConfirm.TryAdd(messageId, tsc);

            return tsc.Task;
        }
    }
}