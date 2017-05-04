namespace TelegramClient.UnitTests.Network
{
    using System.Threading;
    using System.Threading.Tasks;

    using Moq;

    using TelegramClient.Core.Network;
    using TelegramClient.UnitTests.Framework;

    using Xunit;

    public class TcpTransportTests : TestBase
    {
        [Fact]
        public void Send_TcpServiceSendCalled_NotThrows()
        {
            var task = Task.Delay(1);

            var mTcpService = TcpServiceMock.Create().BuildSend(returnTask: () => task);
            this.RegisterMock(mTcpService);

            var seqNo = 1;

            var mSession = SessionMock.Create().BuildGenerateMessageSeqNo(() => seqNo);
            var mClientSettings = ClientSettingsMock.Create().AttachSession(() => mSession.Object);
            this.RegisterMock(mClientSettings);

            this.RegisterType<TcpTransport>();

            // ---

            var transport = this.Resolve<TcpTransport>();
            transport.Send(new byte[1]);
            Thread.Sleep(1000);
            // --

            mTcpService.Verify(service => service.Send(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void Send_ValidTcpMessage_NotThrows()
        {
            var data = new byte[] { 124, 123 };

            var mTcpService = TcpServiceMock.Create().BuildSend(
                bytes =>
                {
                    var message = TcpMessage.Decode(bytes);
                    Assert.Equal(0, message.SequneceNumber);
                    Assert.Equal(data, message.Body);

                });

            this.RegisterMock(mTcpService);

            this.RegisterType<TcpTransport>();

            // ---

            var transport = this.Resolve<TcpTransport>();
            transport.Send(data);
        }

        [Fact]
        public void Receieve_TcpServiceReceieveCalled_NotThrows()
        {
            var data = new byte[] { 123, 123 };

            var mTcpService = TcpServiceMock.Create().BuildReceieve(1, data);
            this.RegisterMock(mTcpService);

            this.RegisterType<TcpTransport>();

            // ---

            var transport = this.Resolve<TcpTransport>();
            var sendTask = transport.Receieve();

            // --
            mTcpService.Verify(service => service.Receieve(), Times.Once);
            Assert.Equal(data, sendTask.Result);
        }
    }
}
