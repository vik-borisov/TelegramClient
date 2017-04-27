namespace TelegramClient.UnitTests.Framework
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Autofac;

    using Moq;

    using TelegramClient.Core.Network;

    internal static class TcpServiceMock
    {
        public static Mock<ITcpService> BuildSend(this Mock<ITcpService> mock, Action<byte[]> callback = null, Func<Task> returnTask = null)
        {
            if (returnTask == null)
            {
                returnTask = () => Task.Delay(1);
            }

            if (callback == null)
            {
                callback = bytes => {};
            }

            mock
                .Setup(service => service.Send(It.IsAny<byte[]>()))
                .Callback(callback)
                .Returns(returnTask);

            return mock;
        }

        public static Mock<ITcpService> BuildReceieve(this Mock<ITcpService> mock, int seqNumber, byte[] body)
        {
            var message = new TcpMessage(seqNumber, body);
            var memoryStream = new MemoryStream(message.Encode());

            mock.BuildReceieve(() => Task.FromResult<Stream>(memoryStream));

            return mock;
        }

        public static Mock<ITcpService> BuildReceieve(this Mock<ITcpService> mock, Func<Task<Stream>> returns)
        {
            mock
                .Setup(service => service.Receieve())
                .Returns(returns);

            return mock;
        }

        public static Mock<ITcpService> Create()
        {
            return new Mock<ITcpService>();
        }

        public static TestBase RegisterTcpService(this TestBase testBase, Mock<ITcpService> mock)
        {
            var builder = testBase.ContainerBuilder;

            builder.RegisterInstance(mock);
            builder.RegisterInstance(mock.Object);

            return testBase;
        }
    }
}