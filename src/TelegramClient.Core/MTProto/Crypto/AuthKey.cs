using System;
using System.IO;
using System.Security.Cryptography;

namespace TelegramClient.Core.MTProto.Crypto
{
    public class AuthKey
    {
        private readonly ulong _auxHash;

        public AuthKey(BigInteger gab)
        {
            Data = gab.ToByteArrayUnsigned();
            using (SHA1 hash = SHA1.Create())
            {
                using (var hashStream = new MemoryStream(hash.ComputeHash(Data), false))
                {
                    using (var hashReader = new BinaryReader(hashStream))
                    {
                        _auxHash = hashReader.ReadUInt64();
                        hashReader.ReadBytes(4);
                        Id = hashReader.ReadUInt64();
                    }
                }
            }
        }

        public AuthKey(byte[] data)
        {
            Data = data;
            using (SHA1 hash = SHA1.Create())
            {
                using (var hashStream = new MemoryStream(hash.ComputeHash(Data), false))
                {
                    using (var hashReader = new BinaryReader(hashStream))
                    {
                        _auxHash = hashReader.ReadUInt64();
                        hashReader.ReadBytes(4);
                        Id = hashReader.ReadUInt64();
                    }
                }
            }
        }

        public byte[] Data { get; }

        public ulong Id { get; }

        public byte[] CalcNewNonceHash(byte[] newNonce, int number)
        {
            using (var stream = new MemoryStream(100))
            {
                using (var bufferWriter = new BinaryWriter(stream))
                {
                    bufferWriter.Write(newNonce);
                    bufferWriter.Write((byte) number);
                    bufferWriter.Write(_auxHash);
                    using (SHA1 sha1 = SHA1.Create())
                    {
                        stream.TryGetBuffer(out var buffer);

                        var hash = sha1.ComputeHash(buffer.Array, 0, (int) stream.Position);
                        var newNonceHash = new byte[16];
                        Array.Copy(hash, 4, newNonceHash, 0, 16);
                        return newNonceHash;
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("(Key: {0}, KeyId: {1}, AuxHash: {2})", Data, Id, _auxHash);
        }
    }
}