namespace TelegramClient.UnitTests.Framework
{
    using System;

    using Moq;

    using TelegramClient.Core.Sessions;

    internal static class SessionMock
    {
        public static Mock<ISession> BuildGetNewMessageId(this Mock<ISession> mock, Func<long> getNewMessageIdFunc)
        {
            mock
                .Setup(service => service.GetNewMessageId())
                .Returns(getNewMessageIdFunc);

            return mock;
        }

        public static Mock<ISession> Create()
        {
            return new Mock<ISession>();
        }
    }
}