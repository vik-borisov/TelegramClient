namespace TelegramClient.UnitTests.Framework
{
    using System;
    using System.Threading.Tasks;

    using Moq;

    using TelegramClient.Core.Network.Confirm;

    internal static class ConfirmationRecieveServiceMock
    {
        public static Mock<IConfirmationRecieveService> BuildWaitForConfirm(this Mock<IConfirmationRecieveService> mock, Func<ulong, Task> waitForConfirmFunc)
        {
            mock.Setup(service => service.WaitForConfirm(It.IsAny<long>()))
                .Returns(waitForConfirmFunc);

            return mock;
        }

        public static Mock<IConfirmationRecieveService> BuildConfirmRequest(this Mock<IConfirmationRecieveService> mock, Action<ulong> confirmRequestFunc)
        {
            mock.Setup(service => service.ConfirmRequest(It.IsAny<long>()))
                .Callback(confirmRequestFunc);

            return mock;
        }

        public static Mock<IConfirmationRecieveService> BuildRequestWithException(this Mock<IConfirmationRecieveService> mock, Action<ulong, Exception> confirmRequestFunc)
        {
            mock.Setup(service => service.RequestWithException(It.IsAny<long>(), It.IsAny<Exception>()))
                .Callback(confirmRequestFunc);

            return mock;
        }

        public static Mock<IConfirmationRecieveService> Create()
        {
            return new Mock<IConfirmationRecieveService>();
        }
    }
}