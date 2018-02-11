namespace TelegramClient.Core.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.MTProto.Crypto;

    public class Step2Response
    {
        public byte[] Nonce { get; set; }

        public byte[] ServerNonce { get; set; }

        public byte[] NewNonce { get; set; }

        public byte[] EncryptedAnswer { get; set; }
    }

    public class Step2DhExchange
    {
        private readonly byte[] _newNonce;

        public Step2DhExchange()
        {
            _newNonce = new byte[32];
        }

        public Step2Response FromBytes(byte[] response)
        {
            using (var responseStream = new MemoryStream(response, false))
            {
                using (var responseReader = new BinaryReader(responseStream))
                {
                    var responseCode = responseReader.ReadUInt32();

                    if (responseCode == 0x79cb045d)
                    {
                        throw new InvalidOperationException("server_DH_params_fail: TODO");
                    }

                    if (responseCode != 0xd0e8075c)
                    {
                        throw new InvalidOperationException($"invalid response code: {responseCode}");
                    }

                    var nonceFromServer = responseReader.ReadBytes(16);

                    // TODO:!
                    /*
					if (!nonceFromServer.SequenceEqual(nonce))
					{
						logger.debug("invalid nonce from server");
						return null;
					}
					*/

                    var serverNonceFromServer = responseReader.ReadBytes(16);

                    // TODO: !
                    /*
					if (!serverNonceFromServer.SequenceEqual(serverNonce))
					{
						logger.error("invalid server nonce from server");
						return null;
					}
					*/

                    var encryptedAnswer = Serializers.Bytes.Read(responseReader);

                    return new Step2Response
                           {
                               EncryptedAnswer = encryptedAnswer,
                               ServerNonce = serverNonceFromServer,
                               Nonce = nonceFromServer,
                               NewNonce = _newNonce
                           };
                }
            }
        }

        public byte[] ToBytes(byte[] nonce, byte[] serverNonce, List<byte[]> fingerprints, BigInteger pq)
        {
            new Random().NextBytes(_newNonce);

            var pqPair = Factorizator.Factorize(pq);

            byte[] reqDhParamsBytes;

            using (var pqInnerData = new MemoryStream(255))
            {
                using (var pqInnerDataWriter = new BinaryWriter(pqInnerData))
                {
                    pqInnerDataWriter.Write(0x83c95aec); // pq_inner_data
                    Serializers.Bytes.Write(pqInnerDataWriter, pq.ToByteArrayUnsigned());
                    Serializers.Bytes.Write(pqInnerDataWriter, pqPair.Min.ToByteArrayUnsigned());
                    Serializers.Bytes.Write(pqInnerDataWriter, pqPair.Max.ToByteArrayUnsigned());
                    pqInnerDataWriter.Write(nonce);
                    pqInnerDataWriter.Write(serverNonce);
                    pqInnerDataWriter.Write(_newNonce);

                    byte[] ciphertext = null;
                    byte[] targetFingerprint = null;
                    foreach (var fingerprint in fingerprints)
                    {
                        pqInnerData.TryGetBuffer(out var buffer);
                        ciphertext = Rsa.Encrypt(BitConverter.ToString(fingerprint).Replace("-", string.Empty), buffer.Array, 0, (int)pqInnerData.Position);
                        if (ciphertext != null)
                        {
                            targetFingerprint = fingerprint;
                            break;
                        }
                    }

                    if (ciphertext == null)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "not found valid key for fingerprints: {0}",
                                string.Join(", ", fingerprints)));
                    }

                    using (var reqDhParams = new MemoryStream(1024))
                    {
                        using (var reqDhParamsWriter = new BinaryWriter(reqDhParams))
                        {
                            reqDhParamsWriter.Write(0xd712e4be); // req_dh_params
                            reqDhParamsWriter.Write(nonce);
                            reqDhParamsWriter.Write(serverNonce);
                            Serializers.Bytes.Write(reqDhParamsWriter, pqPair.Min.ToByteArrayUnsigned());
                            Serializers.Bytes.Write(reqDhParamsWriter, pqPair.Max.ToByteArrayUnsigned());
                            reqDhParamsWriter.Write(targetFingerprint);
                            Serializers.Bytes.Write(reqDhParamsWriter, ciphertext);

                            reqDhParamsBytes = reqDhParams.ToArray();
                        }
                    }
                }

                return reqDhParamsBytes;
            }
        }
    }
}