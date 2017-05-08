namespace TelegramClient.UnitTests.Framework
{
    using System;

    using Moq;

    using TelegramClient.Core.Sessions;

    internal static class SessionStoreMock
    {
        public static Mock<ISessionStore> BuildLoad(this Mock<ISessionStore> mock, Func<ISession> returnsFunc)
        {
            mock
                .Setup(store => store.Load(It.IsAny<string>()))
                .Returns(returnsFunc);

            return mock;
        }

        public static Mock<ISessionStore> BuildSave(this Mock<ISessionStore> mock, Action callbackFunc)
        {
            mock
                .Setup(store => store.Save())
                .Callback(callbackFunc);

            return mock;
        }

        public static Mock<ISessionStore> Create()
        {
            return new Mock<ISessionStore>();
        }
    }
}