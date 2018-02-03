namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Utils;

    [SingleInstance(typeof(ITcpTransport))]
    internal class TcpTransport : ITcpTransport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TcpTransport));

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly ConcurrentQueue<Tuple<byte[], TaskCompletionSource<bool>>> _queue = new ConcurrentQueue<Tuple<byte[], TaskCompletionSource<bool>>>();

        private int _messageSequenceNumber;
         
        public ITcpService TcpService { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public async Task<byte[]> Receieve()
        { 
             
                CancellationToken cancellationToken = default(CancellationToken);
                var packetLengthBytes = new byte[4];
                var readLenghtBytes = await this.TcpService.Read(packetLengthBytes, 0, 4, cancellationToken).ConfigureAwait(false);

                if (readLenghtBytes != 4)
                {
                    throw new InvalidOperationException("Couldn't read the packet length");
                }

                var packetLength = BitConverter.ToInt32(packetLengthBytes, 0);

                var seqBytes = new byte[4];
                var readSeqBytes = await this.TcpService.Read(seqBytes, 0, 4, cancellationToken).ConfigureAwait(false);

                if (readSeqBytes != 4)
                {
                    throw new InvalidOperationException("Couldn't read the sequence");
                }

                var mesSeqNo = BitConverter.ToInt32(seqBytes, 0);

                Log.Debug($"Recieve message with seq_no {mesSeqNo}");

                if (packetLength < 12)
                {
                    throw new InvalidOperationException("Invalid packet length");
                }

                var readBytes = 0;
                var body = new byte[packetLength - 12];
                var neededToRead = packetLength - 12;

                do
                {
                    var bodyByte = new byte[packetLength - 12];
                    var availableBytes = await this.TcpService.Read(bodyByte, 0, neededToRead, cancellationToken).ConfigureAwait(false);

                    neededToRead -= availableBytes;
                    Buffer.BlockCopy(bodyByte, 0, body, readBytes, availableBytes);
                    readBytes += availableBytes;
                }
                while (readBytes < packetLength - 12);

                var crcBytes = new byte[4];
                var readCrcBytes = await this.TcpService.Read(crcBytes, 0, 4, cancellationToken).ConfigureAwait(false);
                if (readCrcBytes != 4)
                {
                    throw new InvalidOperationException("Couldn't read the crc");
                }

                var checksum = BitConverter.ToInt32(crcBytes, 0);

                var rv = new byte[packetLengthBytes.Length + seqBytes.Length + body.Length];

                Buffer.BlockCopy(packetLengthBytes, 0, rv, 0, packetLengthBytes.Length);
                Buffer.BlockCopy(seqBytes, 0, rv, packetLengthBytes.Length, seqBytes.Length);
                Buffer.BlockCopy(body, 0, rv, packetLengthBytes.Length + seqBytes.Length, body.Length);
                var crc32 = new Crc32();
                crc32.SlurpBlock(rv, 0, rv.Length);
                var validChecksum = crc32.Crc32Result;

                if (checksum != validChecksum)
                {
                    throw new InvalidOperationException("invalid checksum! skip");
                }

                return body; 
        }

     

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                TcpService?.Dispose();
            }
        }


        public Task Send(byte[] packet)
        {
            var tcs = new TaskCompletionSource<bool>();

            PushToQueue(packet, tcs);

            return tcs.Task;
        }

        private void PushToQueue(byte[] packet, TaskCompletionSource<bool> tcs)
        {
            _queue.Enqueue(Tuple.Create(packet, tcs));
            SendAllMessagesFromQueue(default(CancellationToken));
        }

        private async Task SendAllMessagesFromQueue(CancellationToken cancellationToken)
        {
            await _semaphoreSlim.WaitAsync().ContinueWith(async _ =>
            {
                if (!_queue.IsEmpty)
                {
                    await (SendFromQueue(cancellationToken).ContinueWith(task => _semaphoreSlim.Release())).ConfigureAwait(false);
                }
                else
                {
                    _semaphoreSlim.Release();
                }
            }).ConfigureAwait(false);
        }

        private async Task SendFromQueue(CancellationToken cancellationToken)
        {
            while (!_queue.IsEmpty)
            {
                _queue.TryDequeue(out var item);

                try
                {
                    await  SendPacket(item.Item1, cancellationToken).ConfigureAwait(false);
                    item.Item2.SetResult(true);
                }
                catch (Exception e)
                {
                    Log.Error("Failed to process the message", e);
                    item.Item2.SetException(e);
                }
            }
        }

        public async Task SendPacket(byte[] packet, CancellationToken cancelationToken)
        {
            var messageSequenceNumber = _messageSequenceNumber++;
            Log.Debug($"Sending message with seq_no {messageSequenceNumber}");
            var tcpMessage = new TcpMessage(messageSequenceNumber, packet);
            var encodedMessage = tcpMessage.Encode();
            await this.TcpService.Send(encodedMessage, cancelationToken).ConfigureAwait(false);
        }


        public async Task Disconnect()
        {
            this._messageSequenceNumber = 0;
            await this.TcpService.Disconnect().ConfigureAwait(false);
        }

        ~TcpTransport()
        {
            Dispose(false);
        }
    }
}