using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TelegramClient.Core.Utils;

namespace TelegramClient.Core.Network
{
    using System.Collections.Concurrent;
    using System.Threading;

    using log4net;

    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;


    internal class TcpTransport : ITcpTransport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TcpTransport));

        private readonly ConcurrentQueue<Tuple<byte[], TaskCompletionSource<TcpMessage>>> _queue = new ConcurrentQueue<Tuple<byte[], TaskCompletionSource<TcpMessage>>>();

        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);

        private readonly IClientSettings _clientSettings;

        private TcpClient _tcpClient;

        public ISessionStore SessionStore { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public TcpTransport(IClientSettings clientSettings)
        {
            _clientSettings = clientSettings;

            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    while (true)
                    {
                        if (_queue.IsEmpty)
                        {
                            _resetEvent.Reset();
                            _resetEvent.Wait();
                        }

                        _queue.TryDequeue(out var item);

                        try
                        {
                            SendPacket(item.Item1).Wait();

                            var recieveTask = ReceievePacket();
                            recieveTask.Wait();
                            var result = recieveTask.Result;

                            item.Item2?.SetResult(result);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Process message failed", e);

                            item.Item2?.SetException(e);
                        }
                    }
                });
        }

        private async Task EnsureClientConnected()
        {
            var session = _clientSettings.Session;

            if (_tcpClient != null)
            {
                var endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;

                if (_tcpClient.Connected && endpoint.Address.ToString() == session.ServerAddress && endpoint.Port == session.Port)
                {
                    return;
                }

                _tcpClient.Dispose();
            }

            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(session.ServerAddress, session.Port);
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
        }

        public Task<TcpMessage> SendAndReceieve(byte[] packet)
        {
            var tsc = new TaskCompletionSource<TcpMessage>();

            var task = tsc.Task;

            PushToQueue(packet, tsc);

            return task;
        }

        public void Send(byte[] packet)
        {
            PushToQueue(packet, null);
        }

        private async Task SendPacket(byte[] packet)
        {
            var mesSeqNo = _clientSettings.Session.GenerateMessageSeqNo();

            Log.Debug($"Send message with seq_no {mesSeqNo}");

            await EnsureClientConnected();

            var tcpMessage = new TcpMessage(mesSeqNo, packet);
            var encodedMessage = tcpMessage.Encode();
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length);

            SessionStore.Save(ClientSettings.Session);
        }

        private void PushToQueue(byte[] packet, TaskCompletionSource<TcpMessage> tsc)
        {
            _queue.Enqueue(Tuple.Create(packet, tsc));

            if (!_resetEvent.IsSet)
            {
                _resetEvent.Set();
            }
        }
        private async Task<TcpMessage> ReceievePacket()
        {
            await EnsureClientConnected();

            var stream = _tcpClient.GetStream();

            var packetLengthBytes = new byte[4];
            if (await stream.ReadAsync(packetLengthBytes, 0, 4) != 4)
                throw new InvalidOperationException("Couldn't read the packet length");

            var packetLength = BitConverter.ToInt32(packetLengthBytes, 0);

            var seqBytes = new byte[4];
            if (await stream.ReadAsync(seqBytes, 0, 4) != 4)
                throw new InvalidOperationException("Couldn't read the sequence");
            var mesSeqNo = BitConverter.ToInt32(seqBytes, 0);

            Log.Debug($"Recieve message with seq_no {mesSeqNo}");

            var readBytes = 0;
            var body = new byte[packetLength - 12];
            var neededToRead = packetLength - 12;

            do
            {
                var bodyByte = new byte[packetLength - 12];
                var availableBytes = await stream.ReadAsync(bodyByte, 0, neededToRead);
                neededToRead -= availableBytes;
                Buffer.BlockCopy(bodyByte, 0, body, readBytes, availableBytes);
                readBytes += availableBytes;
            } while (readBytes != packetLength - 12);

            var crcBytes = new byte[4];
            if (await stream.ReadAsync(crcBytes, 0, 4) != 4)
                throw new InvalidOperationException("Couldn't read the crc");
            var checksum = BitConverter.ToInt32(crcBytes, 0);

            var rv = new byte[packetLengthBytes.Length + seqBytes.Length + body.Length];

            Buffer.BlockCopy(packetLengthBytes, 0, rv, 0, packetLengthBytes.Length);
            Buffer.BlockCopy(seqBytes, 0, rv, packetLengthBytes.Length, seqBytes.Length);
            Buffer.BlockCopy(body, 0, rv, packetLengthBytes.Length + seqBytes.Length, body.Length);
            var crc32 = new Crc32();
            crc32.SlurpBlock(rv, 0, rv.Length);
            var validChecksum = crc32.Crc32Result;

            if (checksum != validChecksum)
                throw new InvalidOperationException("invalid checksum! skip");

            return new TcpMessage(mesSeqNo, body);
        }
    }
}