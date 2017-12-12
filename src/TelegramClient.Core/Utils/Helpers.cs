namespace TelegramClient.Core.Utils
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    using BarsGroup.CodeGuard;

    using OpenTl.Schema;

    using TelegramClient.Core.MTProto.Crypto;

    internal class TlHelpers
    {
        private static readonly Random Random = new Random();

        public static AesKeyData CalcKey(byte[] authKey, byte[] msgKey, bool client)
        {
            Guard.That(authKey.Length, nameof(authKey)).IsEqual(256);
            Guard.That(msgKey.Length, nameof(msgKey)).IsEqual(16);

            var x = client
                        ? 0
                        : 8;

            //sha256_a = SHA256 (msg_key + substr (auth_key, x, 36));
            var sha256ASource = msgKey.Concat(authKey.Skip(x).Take(36)).ToArray();
            var sha256A = Sha256(sha256ASource);

            //sha256_b = SHA256 (substr (auth_key, 40+x, 36) + msg_key);
            var sha256BSource = authKey.Skip(40 + x).Take(36).Concat(msgKey).ToArray();
            var sha256B = Sha256(sha256BSource);

            //aes_key = substr (sha256_a, 0, 8) + substr (sha256_b, 8, 16) + substr (sha256_a, 24, 8);
            var aesKey = sha256A.Take(8).Concat(sha256B.Skip(8).Take(16)).Concat(sha256A.Skip(24).Take(8)).ToArray();

            //aes_iv = substr (sha256_b, 0, 8) + substr (sha256_a, 8, 16) + substr (sha256_b, 24, 8);
            var aesIv = sha256B.Take(8).Concat(sha256A.Skip(8).Take(16)).Concat(sha256B.Skip(24).Take(8)).ToArray();

            return new AesKeyData(aesKey, aesIv);
        }

        public static byte[] CalcMsgKey(byte[] authKey, byte[] data)
        {
            //msg_key_large = SHA256 (substr (auth_key, 88+0, 32) + plaintext + random_padding);
            var msgKeyLarge = Sha256(authKey.Skip(88).Take(32).Concat(data).ToArray());

            //msg_key = substr (msg_key_large, 8, 16);
            return msgKeyLarge.Skip(8).Take(16).ToArray();
        }

        public static byte[] GenerateRandomBytes(int num)
        {
            var data = new byte[num];
            Random.NextBytes(data);
            return data;
        }

        public static int GenerateRandomInt(int maxLengh)
        {
            return Random.Next(maxLengh);
        }

        public static long GenerateRandomLong()
        {
            var rand = ((long)Random.Next() << 32) | Random.Next();
            return rand;
        }

        /// <summary>Generate <see cref="TVector{T}" /> with random long numbers</summary>
        /// <param name="length">Length of list</param>
        /// <returns>Returns a instance of <see cref="TVector{T}" /> with random long numbers</returns>
        /// TODO: Move to  TlHelpers?
        public static TVector<long> GenerateRandomTVectorLong(int length)
        {
            var randomIds = new TVector<long>();
            for (var i = 0; i < length; i++)
            {
                randomIds.Items.Add(GenerateRandomLong());
            }

            return randomIds;
        }

        private static byte[] Sha256(byte[] data)
        {
            using (var sha1 = SHA256.Create())
            {
                return sha1.ComputeHash(data);
            }
        }
    }
}