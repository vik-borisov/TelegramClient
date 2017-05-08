namespace TelegramClient.UnitTests.Framework
{
    using System;

    using Moq;

    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Sessions;

    internal static class SessionMock
    {
        private static readonly Random Random = new Random();

        public static Mock<ISession> BuildGenerateMesId(this Mock<ISession> mock, Func<ulong> getNewMessageIdFunc)
        {
            mock
                .Setup(service => service.GenerateMesId())
                .Returns(getNewMessageIdFunc);

            return mock;
        }

        public static Mock<ISession> BuildGenerateMessageSeqNo(this Mock<ISession> mock, Func<bool, Tuple<ulong, int>> generateMessageSeqNoFunc)
        {
            mock
                .Setup(service => service.GenerateMesIdAndSeqNo(It.IsAny<bool>()))
                .Returns(generateMessageSeqNoFunc);

            return mock;
        }

        public static Mock<ISession> BuildSession(this Mock<ISession> mock, ulong sessionId, ulong salt, byte[] authKeyData)
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