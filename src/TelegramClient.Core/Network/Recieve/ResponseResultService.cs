namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading.Tasks;

    using log4net;

    using Newtonsoft.Json;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Entities;

    [SingleInstance(typeof(IResponseResultGetter), typeof(IResponseResultSetter))]
    internal class ResponseResultService : IResponseResultGetter,
                                           IResponseResultSetter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResponseResultService));

        private readonly ConcurrentDictionary<ulong, TaskCompletionSource<BinaryReader>> _resultCallbacks = new ConcurrentDictionary<ulong, TaskCompletionSource<BinaryReader>>();

        public Task<BinaryReader> Recieve(ulong requestId)
        {
            var tcs = new TaskCompletionSource<BinaryReader>();

            _resultCallbacks[requestId] = tcs;
            return tcs.Task;
        }

        public void ReturnResult(ulong requestId, byte[] bytes)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                var binaryReader = new BinaryReader(new MemoryStream(bytes));
                callback.SetResult(binaryReader);
            }
            else
            {
                Log.Error($"Callback for request with Id {requestId}");
            }
        }

        public void ReturnException(ulong requestId, Exception exception)
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