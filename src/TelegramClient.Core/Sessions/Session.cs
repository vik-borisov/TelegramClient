namespace TelegramClient.Core.Sessions
{
    using System;
    using System.IO;

    using OpenTl.Schema;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.MTProto.Crypto;

    internal class Session : ISession
    {
        private readonly object _syncObject = new object();

        private int _msgIdInc;

        private static readonly Random Random = new Random();

        public string ServerAddress { get; set; }

        public int Port { get; set; }

        public AuthKey AuthKey { get; set; }

        public ulong Id { get; set; }

        public long Salt { get; set; }

        public int TimeOffset { get; set; }

        public int SessionExpires { get; set; }

        public TUser User { get; set; }

        private int SessionSeqNo { get; set; }

        public static Session Create()
        {
            return new Session
                   {
                       Id = ((ulong)Random.Next() << 32) | (ulong)Random.Next(),
                   };
        }
        
        public static Session FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                return null;
            }
            
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                var id = reader.ReadUInt64();
                var sequence = reader.ReadInt32();
                var salt = reader.ReadInt64();
                var timeOffset = reader.ReadInt32();
                var serverAddress = Serializers.String.Read(reader);
                var port = reader.ReadInt32();

                var isAuthExsist = reader.ReadInt32() == 1;
                var sessionExpires = 0;
                TUser tlUser = null;
                if (isAuthExsist)
                {
                    sessionExpires = reader.ReadInt32();
                    tlUser = (TUser)Serializer.DeserializeObject(reader);
                }

                var authData = Serializers.Bytes.Read(reader);

                return new Session
                       {
                           AuthKey = new AuthKey(authData),
                           Id = id,
                           Salt = salt,
                           TimeOffset = timeOffset,
                           SessionSeqNo = sequence,
                           SessionExpires = sessionExpires,
                           User = tlUser,
                           ServerAddress = serverAddress,
                           Port = port
                       };
            }
        }

        public long GenerateMsgId()
        {
            if (_msgIdInc >= 4194303 - 4)
            {
                _msgIdInc = 0;
            }
            else
            {
                _msgIdInc += 4;
            }

            var seconds = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            var newMessageId =
                ((seconds / 1000 + TimeOffset) << 32) |
                ((seconds % 1000) << 22) |
                _msgIdInc;

            return newMessageId;
        }

        public Tuple<long, int> GenerateMsgIdAndSeqNo(bool confirmed)
        {
            lock (_syncObject)
            {
                return Tuple.Create(GenerateMsgId(), GenerateSeqNo(confirmed));
            }
        }

        public byte[] ToBytes()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Id);
                writer.Write(SessionSeqNo);
                writer.Write(Salt);
                writer.Write(TimeOffset);
                Serializers.String.Write(writer, ServerAddress);
                writer.Write(Port);

                if (User != null)
                {
                    writer.Write(1);
                    writer.Write(SessionExpires);
                    var data = Serializer.SerializeObject(User);
                    writer.Write(data);
                }
                else
                {
                    writer.Write(0);
                }

                if (AuthKey != null)
                {
                    Serializers.Bytes.Write(writer, AuthKey.Data);
                }

                return stream.ToArray();
            }
        }

        private int GenerateSeqNo(bool confirmed)
        {
            return confirmed
                       ? SessionSeqNo++ * 2 + 1
                       : SessionSeqNo * 2;
        }
    }
}