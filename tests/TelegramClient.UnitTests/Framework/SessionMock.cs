namespace TelegramClient.UnitTests.Framework
{
    using System;

    using Moq;

    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Sessions;

    internal static class SessionMock
    {
        private static readonly Random Random = new Random();

        public static Mock<ISession> BuildGetNewMessageId(this Mock<ISession> mock, Func<long> getNewMessageIdFunc)
        {
            mock
                .Setup(service => service.GetNewMessageId())
                .Returns(getNewMessageIdFunc);

            return mock;
        }

        public static Mock<ISession> BuildSession(this Mock<ISession> mock, ulong sessionId, ulong salt, Func<int> generateMessageSeqNoFun, byte[] authKeyData)
        {
            mock
                .Setup(session => session.AuthKey)
                .Returns(new AuthKey(authKeyData));

            mock
                .Setup(session => session.Salt)
                .Returns(salt);

            mock
                .Setup(session => session.Id)
                .Returns(sessionId);

            mock
                .Setup(session => session.GenerateMessageSeqNo())
                .Returns(generateMessageSeqNoFun);

            return mock;
        }

        public static byte[] GenerateAuthKeyData()
        {
            var key = new byte[256];
            for (var i = 0; i < 256; i++)
            {
                key[i] = (byte)Random.Next(255);
            }

            return key;
        }

        public static Mock<ISession> Create()
        {
            return new Mock<ISession>();
        }
    }
}