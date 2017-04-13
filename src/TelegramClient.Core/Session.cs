using System;
using System.IO;
using TelegramClient.Core.MTProto;
using TelegramClient.Core.MTProto.Crypto;
using TelegramClient.Entities;
using TelegramClient.Entities.TL;

namespace TelegramClient.Core
{
    public interface ISessionStore
    {
        void Save(Session session);
        Session Load(string sessionUserId);
    }

    public class FileSessionStore : ISessionStore
    {
        public void Save(Session session)
        {
            using (var stream = new FileStream($"{session.SessionUserId}.dat", FileMode.OpenOrCreate))
            {
                var result = session.ToBytes();
                stream.Write(result, 0, result.Length);
            }
        }

        public Session Load(string sessionUserId)
        {
            var sessionFileName = $"{sessionUserId}.dat";
            if (!File.Exists(sessionFileName))
                return null;

            using (var stream = new FileStream(sessionFileName, FileMode.Open))
            {
                var buffer = new byte[2048];
                stream.Read(buffer, 0, 2048);

                return Session.FromBytes(buffer, this, sessionUserId);
            }
        }
    }

    public class FakeSessionStore : ISessionStore
    {
        public void Save(Session session)
        {
        }

        public Session Load(string sessionUserId)
        {
            return null;
        }
    }

    public class Session
    {
        private const string DefaultConnectionAddress = "149.154.175.100"; //"149.154.167.50";

        private const int DefaultConnectionPort = 443;

        private readonly ISessionStore _store;
        private readonly Random _random;

        private Session(ISessionStore store)
        {
            _random = new Random();
            _store = store;
        }

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
                    tlUser = (TlUser) ObjectUtils.DeserializeObject(reader);
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

        public void Save()
        {
            _store.Save(this);
        }

        public static Session TryLoadOrCreateNew(ISessionStore store, string sessionUserId)
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
            var rand = ((ulong) random.Next() << 32) | (ulong) random.Next();
            return rand;
        }

        public long GetNewMessageId()
        {
            var time = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            var newMessageId = ((time / 1000 + TimeOffset) << 32) |
                               ((time % 1000) << 22) |
                               (_random.Next(524288) << 2); // 2^19
            // [ unix timestamp : 32 bit] [ milliseconds : 10 bit ] [ buffer space : 1 bit ] [ random : 19 bit ] [ msg_id type : 2 bit ] = [ msg_id : 64 bit ]

            if (LastMessageId >= newMessageId)
                newMessageId = LastMessageId + 4;

            LastMessageId = newMessageId;
            return newMessageId;
        }
    }
}