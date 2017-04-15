namespace TelegramClient.Core.Sessions
{
    using System;
    using System.IO;

    using global::TelegramClient.Core.MTProto;
    using global::TelegramClient.Core.MTProto.Crypto;
    using global::TelegramClient.Entities;
    using global::TelegramClient.Entities.TL;

    public class Session : ISession
    {
        private const string DefaultConnectionAddress = "149.154.175.100"; //"149.154.167.50";

        private const int DefaultConnectionPort = 443;

        private readonly Random _random;

        private readonly ISessionStore _store;

        public string SessionUserId { get; set; }

        public string ServerAddress { get; set; }

        public int Port { get; set; }

        public AuthKey AuthKey { get; set; }

        public ulong Id { get; set; }

        public int Sequence { get; set; }

        public ulong Salt { get; set; }

        public int TimeOffset { get; set; }

        public long LastMessageId { get; set; }

        public int SessionExpires { get; set; }

        public TlUser TlUser { get; set; }

        private Session(ISessionStore store)
        {
            _random = new Random();
            _store = store;
        }

        public static Session FromBytes(byte[] buffer, ISessionStore store, string sessionUserId)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                var id = reader.ReadUInt64();
                var sequence = reader.ReadInt32();
                var salt = reader.ReadUInt64();
                var lastMessageId = reader.ReadInt64();
                var timeOffset = reader.ReadInt32();
                var serverAddress = Serializers.String.Read(reader);
                var port = reader.ReadInt32();

                var isAuthExsist = reader.ReadInt32() == 1;
                var sessionExpires = 0;
                TlUser tlUser = null;
                if (isAuthExsist)
                {
                    sessionExpires = reader.ReadInt32();
                    tlUser = (TlUser)ObjectUtils.DeserializeObject(reader);
                }

                var authData = Serializers.Bytes.Read(reader);

                return new Session(store)
                       {
                           AuthKey = new AuthKey(authData),
                           Id = id,
                           Salt = salt,
                           Sequence = sequence,
                           LastMessageId = lastMessageId,
                           TimeOffset = timeOffset,
                           SessionExpires = sessionExpires,
                           TlUser = tlUser,
                           SessionUserId = sessionUserId,
                           ServerAddress = serverAddress,
                           Port = port
                       };
            }
        }

        public long GetNewMessageId()
        {
            var time = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            var newMessageId = ((time / 1000 + TimeOffset) << 32) |
                               ((time % 1000) << 22) |
                               (_random.Next(524288) << 2); // 2^19

            // [ unix timestamp : 32 bit] [ milliseconds : 10 bit ] [ buffer space : 1 bit ] [ random : 19 bit ] [ msg_id type : 2 bit ] = [ msg_id : 64 bit ]

            if (LastMessageId >= newMessageId)
            {
                newMessageId = LastMessageId + 4;
            }

            LastMessageId = newMessageId;
            return newMessageId;
        }

        public void Save()
        {
            _store.Save(this);
        }

        public byte[] ToBytes()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Id);
                writer.Write(Sequence);
                writer.Write(Salt);
                writer.Write(LastMessageId);
                writer.Write(TimeOffset);
                Serializers.String.Write(writer, ServerAddress);
                writer.Write(Port);

                if (TlUser != null)
                {
                    writer.Write(1);
                    writer.Write(SessionExpires);
                    ObjectUtils.SerializeObject(TlUser, writer);
                }
                else
                {
                    writer.Write(0);
                }

                Serializers.Bytes.Write(writer, AuthKey.Data);

                return stream.ToArray();
            }
        }

        public static ISession TryLoadOrCreateNew(ISessionStore store, string sessionUserId)
        {
            return store.Load(sessionUserId) ?? new Session(store)
                                                {
                                                    Id = GenerateRandomUlong(),
                                                    SessionUserId = sessionUserId,
                                                    ServerAddress = DefaultConnectionAddress,
                                                    Port = DefaultConnectionPort
                                                };
        }

        private static ulong GenerateRandomUlong()
        {
            var random = new Random();
            var rand = ((ulong)random.Next() << 32) | (ulong)random.Next();
            return rand;
        }
    }
}