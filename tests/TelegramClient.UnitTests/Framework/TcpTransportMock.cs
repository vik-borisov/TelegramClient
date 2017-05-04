namespace TelegramClient.UnitTests.Framework
{
    using System;
    using System.Threading.Tasks;

    using Moq;

    using TelegramClient.Core.Network.Interfaces;

    internal static class TcpTransportMock
    {
        public static Mock<ITcpTransport> BuildSend(this Mock<ITcpTransport> mock, Action<byte[]> callback = null)
        {
            if (callback == null)
            {
                callback = bytes => { };
            }

            mock
                .Setup(service => service.Send(It.IsAny<byte[]>()))
                .Callback(callback);

            return mock;
        }

        public static Mock<ITcpTransport> BuildReceieve(this Mock<ITcpTransport> mock, Func<Task<byte[]>> returns)
        {
            mock
                .Setup(service => service.Receieve())
                .Returns(returns);

            return mock;
        }

        public static Mock<ITcpTransport> Create()
        {
            return new Mock<ITcpTransport>();
        }
    }
}