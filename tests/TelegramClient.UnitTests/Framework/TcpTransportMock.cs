namespace TelegramClient.UnitTests.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Autofac;

    using Moq;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Network.Tcp;

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

        public static void RegisterAdapterForHandler(this TestBase testBase)
        {
            testBase.ContainerBuilder.RegisterAdapter<IEnumerable<IRecieveHandler>, Dictionary<uint, IRecieveHandler>>(handlers =>
            {
                var handlerMap = new Dictionary<uint, IRecieveHandler>();
                foreach (var handler in handlers.ToArray())
                {
                    foreach (var handleCode in handler.HandleCodes)
                    {
                        handlerMap[handleCode] = handler;
                    }
                }

                return handlerMap;
            });
        }

        public static Mock<ITcpTransport> BuildReceieve(this Mock<ITcpTransport> mock, Func<Task<byte[]>> returns)
        {
            mock
                .Setup(service => service.Receieve())
                .Returns(returns);

            return mock;
        }

        public static Mock<ITcpTransport> BuildReceieve(this Mock<ITcpTransport> mock, out TaskCompletionSource<byte[]> tsc)
        {
            var source = tsc = new TaskCompletionSource<byte[]>();
            var infiniteWait = new TaskCompletionSource<byte[]>();

            return mock.BuildReceieve(() => !source.Task.IsCompleted && !source.Task.IsFaulted && !source.Task.IsCanceled ? source.Task : infiniteWait.Task);
        }

        public static Mock<ITcpTransport> Create()
        {
            return new Mock<ITcpTransport>();
        }
    }
}