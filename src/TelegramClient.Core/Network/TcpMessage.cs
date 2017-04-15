using System;
using System.IO;
using TelegramClient.Core.Utils;

namespace TelegramClient.Core.Network
{
    using BarsGroup.CodeGuard;
    using BarsGroup.CodeGuard.Validators;

    public class TcpMessage
    {
        private readonly int _sequneceNumber;

        public byte[] Body { get; }

        public TcpMessage(int seqNumber, byte[] body)
        {
            Guard.That(body, nameof(body)).IsNotNull();

            _sequneceNumber = seqNumber;
            Body = body;
        }


        public byte[] Encode()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    // https://core.telegram.org/mtproto#tcp-transport
                    /*
                        4 length bytes are added at the front 
                        (to include the length, the sequence number, and CRC32; always divisible by 4)
                        and 4 bytes with the packet sequence number within this TCP connection 
                        (the first packet sent is numbered 0, the next one 1, etc.),
                        and 4 CRC32 bytes at the end (length, sequence number, and payload together).
                    */
                    binaryWriter.Write(Body.Length + 12);
                    binaryWriter.Write(_sequneceNumber);
                    binaryWriter.Write(Body);
                    var crc32 = new Crc32();

                    memoryStream.TryGetBuffer(out var buffer);
                    crc32.SlurpBlock(buffer.Array, 0, 8 + Body.Length);

                    binaryWriter.Write(crc32.Crc32Result);

                    var transportPacket = memoryStream.ToArray();

                    //					Debug.WriteLine("Tcp packet #{0}\n{1}", SequneceNumber, BitConverter.ToString(transportPacket));

                    return transportPacket;
                }
            }
        }

        public static TcpMessage Decode(byte[] body)
        {
            Guard.That(body, nameof(body)).IsNotNull();
            Guard.That(body.Length, nameof(body.Length)).IsLessThan(12);

            using (var memoryStream = new MemoryStream(body))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    var packetLength = binaryReader.ReadInt32();

                    if (packetLength < 12)
                        throw new InvalidOperationException(string.Format("invalid packet length: {0}", packetLength));

                    var seq = binaryReader.ReadInt32();
                    var packet = binaryReader.ReadBytes(packetLength - 12);
                    var checksum = binaryReader.ReadInt32();

                    var crc32 = new Crc32();
                    crc32.SlurpBlock(body, 0, packetLength - 4);
                    var validChecksum = crc32.Crc32Result;

                    if (checksum != validChecksum)
                        throw new InvalidOperationException("invalid checksum! skip");

                    return new TcpMessage(seq, packet);
                }
            }
        }
    }
}