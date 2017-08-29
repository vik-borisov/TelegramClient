namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading.Tasks;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(IResponseResultGetter), typeof(IResponseResultSetter))]
    internal class ResponseResultService : IResponseResultGetter,
                                           IResponseResultSetter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResponseResultService));

        private readonly ConcurrentDictionary<long, TaskCompletionSource<IObject>> _resultCallbacks = new ConcurrentDictionary<long, TaskCompletionSource<IObject>>();

        public Task<IObject> Recieve(long requestId)
        {
            var tcs = new TaskCompletionSource<IObject>();

            _resultCallbacks[requestId] = tcs;
            return tcs.Task;
        }

        public void ReturnResult(long requestId, IObject obj)
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
    }
}