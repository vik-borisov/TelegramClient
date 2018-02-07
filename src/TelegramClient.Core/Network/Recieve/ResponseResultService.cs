namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(IResponseResultGetter), typeof(IResponseResultSetter))]
    internal class ResponseResultService : IResponseResultGetter,
                                           IResponseResultSetter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResponseResultService));

        private readonly ConcurrentDictionary<long, TaskCompletionSource<object>> _resultCallbacks = new ConcurrentDictionary<long, TaskCompletionSource<object>>();

        private readonly IMemoryCache _tokensCache;

        public ResponseResultService()
        {
            _tokensCache = new MemoryCache(new OptionsManager<MemoryCacheOptions>(new []{new ConfigureOptions<MemoryCacheOptions>(options => options.ExpirationScanFrequency = TimeSpan.FromMinutes(1)), }));
        }
        
        public Task<object> Receive(long requestId)
        {
            var tcs = new TaskCompletionSource<object>();

            var token = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;

            _tokensCache.Set(requestId, token);
            
            token.Register(
                () =>
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        Log.Warn($"Message response result timed out for messageid '{requestId}'");
                        
                        _resultCallbacks.TryRemove(requestId, out var _);
                        
                        tcs.TrySetCanceled(token);
                    }
                });

            _resultCallbacks[requestId] = tcs;
            return tcs.Task;
        }

        public void ReturnException(long requestId, Exception exception)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
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

            foreach (var value in _resultCallbacks.Values)
            {
                value.SetException(exception);
            }
        }

        public void ReturnResult(long requestId, object obj)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
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