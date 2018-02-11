namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using NullGuard;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Utils;

    [SingleInstance(typeof(ITcpTransport))]
    internal class TcpTransport : ITcpTransport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TcpTransport));

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ConcurrentQueue<(byte[], TaskCompletionSource<bool>, CancellationToken)> _messageQueue =
            new ConcurrentQueue<(byte[], TaskCompletionSource<bool>, CancellationToken)>();

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private int _messageSequenceNumber;

        public ITcpService TcpService { get; set; }

        public async Task Disconnect()
        {
            _messageSequenceNumber = 0;
            await TcpService.Disconnect().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<byte[]> Receieve()
        {
            var cancellationToken = default(CancellationToken);
            var packetLengthBytes = new byte[4];
            var readLenghtBytes = await TcpService.Read(packetLengthBytes, 0, 4, cancellationToken).ConfigureAwait(false);

            if (readLenghtBytes != 4)
            {
                throw new InvalidOperationException("Couldn't read the packet length");
            }

            var packetLength = BitConverter.ToInt32(packetLengthBytes, 0);

            var seqBytes = new byte[4];
            var readSeqBytes = await TcpService.Read(seqBytes, 0, 4, cancellationToken).ConfigureAwait(false);

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
                var availableBytes = await TcpService.Read(bodyByte, 0, neededToRead, cancellationToken).ConfigureAwait(false);

                neededToRead -= availableBytes;
                Buffer.BlockCopy(bodyByte, 0, body, readBytes, availableBytes);
                readBytes += availableBytes;
            }
            while (readBytes < packetLength - 12);

            var crcBytes = new byte[4];
            var readCrcBytes = await TcpService.Read(crcBytes, 0, 4, cancellationToken).ConfigureAwait(false);
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

        public Task Send(byte[] packet, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            PushToQueue(packet, tcs, cancellationToken);

            return tcs.Task;
        }

        public async Task SendPacket(byte[] packet, CancellationToken cancelationToken)
        {
            var messageSequenceNumber = _messageSequenceNumber++;

            Log.Debug($"Sending message with seq_no {messageSequenceNumber}");

            var tcpMessage = new TcpMessage(messageSequenceNumber, packet);

            var encodedMessage = tcpMessage.Encode();

            await TcpService.Send(encodedMessage, cancelationToken).ConfigureAwait(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Cancel();
                TcpService?.Dispose();
            }
        }

        private void PushToQueue(byte[] packet, TaskCompletionSource<bool> tcs, CancellationToken cancellationToken)
        {
            _messageQueue.Enqueue((packet, tcs, cancellationToken));
            SendAllMessagesFromQueue().ConfigureAwait(false);
        }

        private async Task SendAllMessagesFromQueue()
        {
            await _semaphoreSlim.WaitAsync();
            if (!_messageQueue.IsEmpty)
            {
                await SendFromQueue().ContinueWith(task => _semaphoreSlim.Release()).ConfigureAwait(false);
            }
            else
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task SendFromQueue()
        {
            while (!_messageQueue.IsEmpty)
            {
                _messageQueue.TryDequeue(out var item);
                (byte[] message, TaskCompletionSource<bool> tcs, CancellationToken token) = item;

                try
                {
                    await SendPacket(message, token).ConfigureAwait(false);
                    tcs.SetResult(true);
                }
                catch (Exception e)
                {
                    Log.Error("Failed to process the message", e);

                    tcs.SetException(e);
                }
            }
        }

        ~TcpTransport()
        {
            Dispose(false);
        }
    }
}