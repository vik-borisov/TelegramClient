﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TelegramClient.Core.Utils;

namespace TelegramClient.Core.Network
{
    using log4net;

    using TelegramClient.Core.Settings;


    internal class TcpTransport : ITcpTransport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TcpTransport));

        private readonly IClientSettings _clientSettings;

        private TcpClient _tcpClient;
        private int _mesSeqNo;


        public TcpTransport(IClientSettings clientSettings)
        {
            _clientSettings = clientSettings;
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
            _mesSeqNo = 0;
            await _tcpClient.ConnectAsync(session.ServerAddress, session.Port);
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
        }

        public async Task Send(byte[] packet)
        {
            Log.Debug($"Send message with seq_no {_mesSeqNo}");

            await EnsureClientConnected();

            var tcpMessage = new TcpMessage(_mesSeqNo, packet);
            var encodedMessage = tcpMessage.Encode();
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length);
            _mesSeqNo++;
        }

        public async Task<TcpMessage> Receieve()
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
            var seq = BitConverter.ToInt32(seqBytes, 0);

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

            return new TcpMessage(seq, body);
        }
    }
}