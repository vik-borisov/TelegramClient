namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IConfirmationRecieveService))]
    internal class ConfirmationRecieveService : IConfirmationRecieveService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfirmationRecieveService));

        private readonly ConcurrentDictionary<long, (Timer, TaskCompletionSource<bool>)> _waitConfirm = new ConcurrentDictionary<long, (Timer, TaskCompletionSource<bool>)>();

        public void ConfirmRequest(long requestId)
        {
            if (_waitConfirm.TryGetValue(requestId, out var data))
            {
                (Timer timer, TaskCompletionSource<bool> tcs) = data;
                timer.Dispose();
                
                tcs.TrySetResult(true);
                _waitConfirm.TryRemove(requestId, out var _);
            }
        }

        public void RequestWithException(long requestId, Exception exception)
        {
            if (_waitConfirm.TryGetValue(requestId, out var data))
            {
                (Timer timer, TaskCompletionSource<bool> tcs) = data;
                timer.Dispose();
                
                tcs.TrySetException(exception);
                
                _waitConfirm.TryRemove(requestId, out var _);
            }
        }

        public Task WaitForConfirm(long messageId)
        {
            var tcs = new TaskCompletionSource<bool>();
            
            var timer = new Timer( _ =>
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        Log.Warn($"Message confirmation timed out for messageid '{messageId}'");
                        
                        _waitConfirm.TryRemove(messageId, out var _);
                        
                        tcs.TrySetCanceled(new CancellationTokenSource().Token);
                    }
                }, null, TimeSpan.FromMinutes(1), TimeSpan.Zero);

            _waitConfirm.TryAdd(messageId, (timer, tcs));

            return tcs.Task;
        }
    }
}