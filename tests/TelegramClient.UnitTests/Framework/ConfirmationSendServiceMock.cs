namespace TelegramClient.UnitTests.Framework
{
    using System;
    using System.Threading.Tasks;

    using Moq;

    using TelegramClient.Core.Network.Interfaces;

    internal static class ConfirmationSendServiceMock
    {
        public static Mock<IConfirmationSendService> BuildAddForSend(this Mock<IConfirmationSendService> mock, Action<ulong> addForSendFunc)
        {
            mock.Setup(service => service.AddForSend(It.IsAny<ulong>()))
                .Callback(addForSendFunc);

            return mock;
        }

        public static Mock<IConfirmationSendService> Create()
        {
            return new Mock<IConfirmationSendService>();
        }
    }
}