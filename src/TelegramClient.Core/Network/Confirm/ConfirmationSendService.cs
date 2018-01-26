namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Interfaces;

    [SingleInstance(typeof(IConfirmationSendService))]
    internal class ConfirmationSendService : IConfirmationSendService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfirmationSendService));

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly ConcurrentQueue<long> _waitSendConfirmation = new ConcurrentQueue<long>();

        public IMtProtoSender MtProtoSender { get; set; }

        private async Task SendFromQueue()
        {
            while (!_waitSendConfirmation.IsEmpty)
            {
                var msgs = new HashSet<long>();
                while (!_waitSendConfirmation.IsEmpty)
                {
                    _waitSendConfirmation.TryDequeue(out var item);
                    msgs.Add(item);
                }

                try
                {
                    Log.Debug($"Sending confirmation for messages {string.Join(",", msgs.Select(m => m.ToString()))}");

                    var message = new TMsgsAck
                    {
                        MsgIds = new TVector<long>(msgs.ToArray())
                    };

                    await MtProtoSender.Send(message);
                }
                catch (Exception e)
                {
                    Log.Error("Process message failed", e);
                }
            }
        }

        public void AddForSend(long messageId)
        {
            _waitSendConfirmation.Enqueue(messageId);

            SendAllMessagesFromQueue();
        }


        public async void SendAllMessagesFromQueue()
        {
            await _semaphoreSlim.WaitAsync().ContinueWith(async _ =>
             {
                 if (!_waitSendConfirmation.IsEmpty)
                 {
					 try
					 {
						 await SendFromQueue().ConfigureAwait(false);
					 }
					 catch(Exception ex)
					 {
						 Log.Error("Failed to send message", ex);
					 }
					 finally
					 {
						 _semaphoreSlim.Release();
					 }
                 }
                 else
                 {
                     _semaphoreSlim.Release();
                 }
             }).ConfigureAwait(false);
        }
    }
}