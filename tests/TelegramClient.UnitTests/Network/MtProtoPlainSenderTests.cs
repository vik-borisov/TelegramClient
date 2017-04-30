namespace TelegramClient.UnitTests.Network
{
    using System.IO;
    using System.Threading.Tasks;

    using Moq;

    using TelegramClient.Core.Network;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;
    using TelegramClient.UnitTests.Framework;

    using Xunit;

    public class MtProtoPlainSenderTests: TestBase
    {
        [Fact]
        public void SendAndRecive_SimpleCall_NotThrows()
        {
            var sendData = new byte[] { 123, 214 };
            const long SendMessageId = 1234;

            var receiveData = new byte[] { 123, 214 };
            const long ReciveMessageId = 12;
            const long AuthKeyId = 12123123;

            var mTcpTransport = TcpTransportMock.Create();
            AddSendHandler(mTcpTransport, SendMessageId, sendData);
           // AddReceiveHandler(mTcpTransport, AuthKeyId, ReciveMessageId, receiveData);

            this.RegisterMock(mTcpTransport);

            var mSession = SessionMock.Create().BuildGetNewMessageId(() => SendMessageId);
            var mClientSettings = ClientSettingsMock.Create().AttachSession(() => mSession.Object);
            this.RegisterMock(mClientSettings);

          

            this.RegisterType<MtProtoPlainSender>();

            // ---

            var mtProtoPlainSender = this.Resolve<MtProtoPlainSender>();
            var sendTask = mtProtoPlainSender.SendAndReceive(sendData);

            // --

            Assert.Equal(receiveData, sendTask.Result);
            mTcpTransport.Verify(transport => transport.Send(It.IsAny<byte[]>()), Times.Once);
        }

        private void AddSendHandler(Mock<ITcpTransport> mock, long messageId, byte[] sendData)
        {
            mock
                .BuildSend(
                    bytes =>
                    {
                        using (var memoryStream = new MemoryStream(bytes))
                        {
                            using (var binaryReader = new BinaryReader(memoryStream))
                            {
                                Assert.Equal(0, binaryReader.ReadInt64());
                                Assert.Equal(messageId, binaryReader.ReadInt64());
                                Assert.Equal(sendData.Length, binaryReader.ReadInt32());
                                Assert.Equal(sendData, binaryReader.ReadBytes(sendData.Length));
                            }
                        }
                    });
        }

        //private void AddReceiveHandler(Mock<ITcpTransport> mock, long authKeyId, long messageId, byte[] reciveData)
        //{
        //    mock
        //        .BuildReceieve(() =>
        //            {
        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    using (var binaryWriter = new BinaryWriter(memoryStream))
        //                    {
        //                        binaryWriter.Write(authKeyId);
        //                        binaryWriter.Write(messageId);
        //                        binaryWriter.Write(reciveData.Length);
        //                        binaryWriter.Write(reciveData);

        //                        return Task.FromResult(memoryStream.ToArray());
        //                    }
        //                }
        //            });
        //}
    }
}
