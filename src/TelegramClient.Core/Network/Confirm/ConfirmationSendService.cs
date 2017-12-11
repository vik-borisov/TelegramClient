namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Interfaces;

    [SingleInstance(typeof(IConfirmationSendService))]
    internal class ConfirmationSendService : IConfirmationSendService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfirmationSendService));

        private readonly ConcurrentQueue<long> _waitSendConfirmation = new ConcurrentQueue<long>();
        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);

        public IMtProtoSender MtProtoSender { get; set; }

        private readonly BackgroundWorker _worker = new BackgroundWorker();

        public ConfirmationSendService()
        {
            _worker.DoWork += (sender, args) =>
            {
                
                while (true)
                {
                    if (_waitSendConfirmation.IsEmpty)
                    {
                        _resetEvent.Reset();
                        _resetEvent.Wait();
                    }

                    if (args.Cancel)
                    {
                        return;
                    }

                    var msgs = new HashSet<long>();
                    while (!_waitSendConfirmation.IsEmpty)
                    {
                        _waitSendConfirmation.TryDequeue(out var item);
                        msgs.Add(item);
                    }

                    try
                    {
                        Log.Debug($"Sending confirmation for messages {string.Join(",", msgs.Select(m => m.ToString()))}");

                        var message = new TMsgsAck()
                                      {
                                          MsgIds = new TVector<long>(msgs.ToArray())
                                      };

                        MtProtoSender.Send(message);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Process message failed", e);
                    }
                }
            };
            _worker.RunWorkerAsync();
        }

        public void AddForSend(long messageId)
        {
            _waitSendConfirmation.Enqueue(messageId);

            if (!_resetEvent.IsSet)
            {
                _resetEvent.Set();
            }
        }

        public void Dispose()
        {
            _resetEvent?.Dispose();
            _worker?.CancelAsync();
            _worker?.Dispose();
        }
    }
}