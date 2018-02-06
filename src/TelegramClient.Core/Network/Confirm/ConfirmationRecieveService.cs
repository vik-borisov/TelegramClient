namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;

    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IConfirmationRecieveService))]
    internal class ConfirmationRecieveService : IConfirmationRecieveService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfirmationRecieveService));

        private readonly ConcurrentDictionary<long, TaskCompletionSource<bool>> _waitConfirm = new ConcurrentDictionary<long, TaskCompletionSource<bool>>();

        private readonly IMemoryCache _tokensCache;

        public ConfirmationRecieveService()
        {
            _tokensCache = new MemoryCache(new OptionsManager<MemoryCacheOptions>(new []{new ConfigureOptions<MemoryCacheOptions>(options => options.ExpirationScanFrequency = TimeSpan.FromSeconds(10)), }));
        }
        
        public void ConfirmRequest(long requestId)
        {
            if (_waitConfirm.TryGetValue(requestId, out var tsc))
            {
                tsc.TrySetResult(true);
                _waitConfirm.TryRemove(requestId, out var _);
            }
        }

        public void RequestWithException(long requestId, Exception exception)
        {
            if (_waitConfirm.TryGetValue(requestId, out var tsc))
            {
                tsc.TrySetException(exception);
                _waitConfirm.TryRemove(requestId, out var _);
            }
        }

        public Task WaitForConfirm(long messageId)
        {
            var tsc = new TaskCompletionSource<bool>();
            var token = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
            _tokensCache.Set(messageId, token);
            
            token.Register(
                () =>
                {
                    if (!tsc.Task.IsCompleted)
                    {
                        Log.Warn($"Message confirmation timed out for messageid '{messageId}'");
                        _waitConfirm.TryRemove(messageId, out var _);
                        tsc.TrySetCanceled(token);
                    }
                });

            _waitConfirm.TryAdd(messageId, tsc);

            return tsc.Task;
        }
    }
}