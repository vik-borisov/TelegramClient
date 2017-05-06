namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.Network.Recieve.Interfaces;

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

        public void ReturnResult(ulong requestId, byte[] reader)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                var stream = new MemoryStream(reader);
                var binaryReader = new BinaryReader(stream);
                callback.SetResult(binaryReader);
            }
            else
            {
                Log.Debug($"Request with Id {requestId} wasn't not handled");
            }
        }

        public void ReturnException(ulong requestId, Exception exception)
        {
            if (_resultCallbacks.TryGetValue(requestId, out var callback))
            {
                callback.SetException(exception);
            }
            else
            {
                Log.Debug($"Request with Id {requestId} wasn't not handled");
            }
        }
    }
}