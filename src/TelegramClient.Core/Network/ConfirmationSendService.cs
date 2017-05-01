namespace TelegramClient.Core.Network
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using log4net;

    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Requests;

    internal class ConfirmationSendService : IConfirmationSendService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfirmationSendService));

        private readonly ConcurrentQueue<ulong> _waitSendConfirmation = new ConcurrentQueue<ulong>();
        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);


        public IMtProtoSender MtProtoSender { get; set; }

        public void StartSendingConfirmation()
        {
            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    while (true)
                    {
                        if (_waitSendConfirmation.IsEmpty)
                        {
                            _resetEvent.Reset();
                            _resetEvent.Wait();
                        }
                        var msgs = new HashSet<ulong>();
                        while (!_waitSendConfirmation.IsEmpty)
                        {
                            _waitSendConfirmation.TryDequeue(out var item);
                            msgs.Add(item);
                        }

                        try
                        {
                            Log.Debug($"Sending confirmation for messages {string.Join(",", msgs.Select(m => m.ToString()))}");

                            MtProtoSender.Send(new AckRequest(msgs));
                        }
                        catch (Exception e)
                        {
                            Log.Error("Process message failed", e);
                        }
                    }
                });
        }

        public void AddForSend(ulong messageId)
        {
            _waitSendConfirmation.Enqueue(messageId);

            if (!_resetEvent.IsSet)
            {
                _resetEvent.Set();
            }
        }
    }
}