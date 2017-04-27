using System;
using System.Threading.Tasks;
using TelegramClient.Core.Utils;

namespace TelegramClient.Core.Network
{
    internal class TcpTransport : ITcpTransport
    {
        private int _sendCounter;

        public ITcpService TcpService { get; set; }

        public Task Send(byte[] packet)
        {
            var tcpMessage = new TcpMessage(_sendCounter, packet);
            var encodedMessage = tcpMessage.Encode();

            _sendCounter++;

            return TcpService.Send(encodedMessage);
        }

        public async Task<byte[]> Receieve()
        {
            var stream = await TcpService.Receieve();

            var packetLengthBytes = new byte[4];
            var readLenghtBytes = await stream.ReadAsync(packetLengthBytes, 0, 4);

            if (readLenghtBytes != 4)
                throw new InvalidOperationException("Couldn't read the packet length");
            var packetLength = BitConverter.ToInt32(packetLengthBytes, 0);

            var seqBytes = new byte[4];
            var readSeqBytes = await stream.ReadAsync(seqBytes, 0, 4);

            if (readSeqBytes != 4)
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
            var readCrcBytes = await stream.ReadAsync(crcBytes, 0, 4);
            if (readCrcBytes != 4)
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

            return body;
        }
    }
}