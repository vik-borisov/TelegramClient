namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(IResponseResultGetter), typeof(IResponseResultSetter))]
    internal class ResponseResultService : IResponseResultGetter,
                                           IResponseResultSetter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResponseResultService));

        private readonly ConcurrentDictionary<long, (Timer, TaskCompletionSource<object>)> _resultCallbacks = new ConcurrentDictionary<long, (Timer, TaskCompletionSource<object>)>();

      public Task<object> Receive(long requestId)
        {
            var tcs = new TaskCompletionSource<object>();

            var timer = new Timer( _ =>
            {
                if (!tcs.Task.IsCompleted)
                {
                    Log.Warn($"Message response result timed out for messageid '{requestId}'");
                        
                    _resultCallbacks.TryRemove(requestId, out var _);
                        
                    tcs.TrySetCanceled(new CancellationTokenSource().Token);
                }
            }, null, TimeSpan.FromMinutes(1), TimeSpan.Zero);

            _resultCallbacks[requestId] = (timer, tcs);
            return tcs.Task;
        }

        public void ReturnException(long requestId, Exception exception)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var data))
            {
                (Timer timer, TaskCompletionSource<object> callback) = data;
                timer.Dispose();
                
                callback.TrySetException(exception);
                
                Log.Error($"Request was processed with error", exception);
                
                _resultCallbacks.TryRemove(requestId, out var response);
            }
            else
            {
                Log.Error($"Callback for request with Id {requestId} wasn't found");
            }
        }

        public void ReturnException(Exception exception)
        {
            Log.Error($"All requests was processed with error", exception);

            foreach ((Timer timer, TaskCompletionSource<object> callback) in _resultCallbacks.Values)
            {
                timer.Dispose();
                callback.TrySetException(exception);
            }
        }

        public void ReturnResult(long requestId, object obj)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var data))
            {
                (Timer timer, TaskCompletionSource<object> callback) = data;
                timer.Dispose();
                
                callback.TrySetResult(obj);
                
                _resultCallbacks.TryRemove(requestId, out var _);
            }
            else
            {
                Log.Error($"Callback for request with Id {requestId}");
            }
        }
    }
}