namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(IResponseResultGetter), typeof(IResponseResultSetter))]
    internal class ResponseResultService : IResponseResultGetter,
                                           IResponseResultSetter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResponseResultService));

        private readonly ConcurrentDictionary<long, TaskCompletionSource<object>> _resultCallbacks = new ConcurrentDictionary<long, TaskCompletionSource<object>>();

        public Task<object> Recieve(long requestId)
        {
            var tcs = new TaskCompletionSource<object>();

            _resultCallbacks[requestId] = tcs;
            return tcs.Task;
        }

        public void ReturnException(long requestId, Exception exception)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                callback.SetException(exception);
                Log.Error($"Request was processed with error", exception);
            }
            else
            {
                Log.Error($"Callback for request with Id {requestId} wasn't found");
            }
        }

        public void ReturnResult(long requestId, object obj)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                callback.SetResult(obj);
            }
            else
            {
                Log.Error($"Callback for request with Id {requestId}");
            }
        }
    }
}