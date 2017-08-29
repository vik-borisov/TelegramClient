namespace TelegramClient.UnitTests.Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Moq;

    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Sessions;

    internal static class RecieveHandlerMock
    {
        public static Mock<IRecieveHandler> BuildRecieveHandler(this Mock<IRecieveHandler> mock, uint[] code)
        {
            mock
                .Setup(recieveHandler => recieveHandler.HandleCodes)
                .Returns(code);

            return mock;
        }

        public static Mock<IRecieveHandler> BuildHandleResponce(this Mock<IRecieveHandler> mock, Func<uint, BinaryReader, byte[]> callback)
        {
            mock
                .Setup(recieveHandler => recieveHandler.HandleResponce(It.IsAny<BinaryReader>()))
                .Returns(callback);

            return mock;
        }

        public static Mock<IRecieveHandler> Create()
        {
            return new Mock<IRecieveHandler>();
        }
    }
}