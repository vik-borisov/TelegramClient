namespace TelegramClient.Core.Sessions
{
    using System;
    using System.IO;
    using System.Threading;

    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Entities;
    using TelegramClient.Entities.TL;

    public class Session : ISession
    {
        private readonly object _syncObject = new object();

        private int _inc;

        private int _messageSeqNo;

        public string SessionUserId { get; set; }

        public string ServerAddress { get; set; }

        public int Port { get; set; }

        public AuthKey AuthKey { get; set; }

        public ulong Id { get; set; }

        private int _sessionSeqNo;
        private int SessionSeqNo
        {
            get => _sessionSeqNo;
            set => _sessionSeqNo = value;
        }

        public ulong Salt { get; set; }

        public int TimeOffset { get; set; }

        public int SessionExpires { get; set; }

        public TlUser TlUser { get; set; }

        public static Session FromBytes(byte[] buffer, string sessionUserId)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                var id = reader.ReadUInt64();
                var sequence = reader.ReadInt32();
                var salt = reader.ReadUInt64();
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

                return new Session
                       {
                           AuthKey = new AuthKey(authData),
                           Id = id,
                           Salt = salt,
                           TimeOffset = timeOffset,
                           SessionSeqNo = sequence,
                           SessionExpires = sessionExpires,
                           TlUser = tlUser,
                           SessionUserId = sessionUserId,
                           ServerAddress = serverAddress,
                           Port = port
                       };
            }
        }

        public int GenerateSessionSeqNo(bool confirmed)
        {
            if (confirmed)
            {
                var result =  SessionSeqNo * 2 + 1;
                Interlocked.Increment(ref _sessionSeqNo);

                return result;
            }

            return SessionSeqNo * 2;
        }

        public int GenerateMessageSeqNo()
        {
            var result = _messageSeqNo;
            Interlocked.Increment(ref _messageSeqNo);

            return result;
        }

        public ulong GetNewMessageId()
        {

            lock (_syncObject)
            {
                if (_inc >= 4194303 - 4)
                {
                    _inc = 0;
                }
                else
                {
                    _inc += 4;
                }
            }

            var seconds = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;


            var newMessageId = 
                    ((seconds / 1000 + TimeOffset) << 32) |
                    ((seconds % 1000) << 22) |
                    _inc;

            return (ulong) newMessageId;
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

                if (AuthKey != null)
                {
                    Serializers.Bytes.Write(writer, AuthKey.Data);
                }

                return stream.ToArray();
            }
        }
    }
}