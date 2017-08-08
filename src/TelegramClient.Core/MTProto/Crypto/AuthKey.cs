using System;
using System.IO;
using System.Security.Cryptography;

namespace TelegramClient.Core.MTProto.Crypto
{
    using OpenTl.Common.Crypto;

    public class AuthKey
    {
        private readonly ulong _auxHash;

        public AuthKey(byte[] data)
        {
            Data = data;
            var hash = SHA1Helper.ComputeHashsum(Data);
            _auxHash = BitConverter.ToUInt64(hash, 0);
            Id = BitConverter.ToUInt64(hash, 12);
        }

        public byte[] Data { get; }

        public ulong Id { get; }

        public override string ToString()
        {
            return string.Format("(Key: {0}, KeyId: {1}, AuxHash: {2})", Data, Id, _auxHash);
        }
    }
}