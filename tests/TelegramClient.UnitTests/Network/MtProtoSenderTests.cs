namespace TelegramClient.UnitTests.Network
{
    using System.Threading.Tasks;

    using Autofac;

    using Moq;

    using TelegramClient.Core.Helpers;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network;
    using TelegramClient.Core.Requests;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;
    using TelegramClient.Entities;
    using TelegramClient.UnitTests.Framework;

    using Xunit;


    public class MtProtoSenderTests : TestBase
    {

        [Fact]
        public void SendAndRecive_SimpleCall_NotThrows()
        {
            const long SendMessageId = 1234;
            var authKeyData = SessionMock.GenerateAuthKeyData();
            ulong sessionId = 123456;
            ulong salt = 654321;
            var mSession = SessionMock.Create()
                                      .BuildGetNewMessageId(() => SendMessageId)
                                      .BuildSession(sessionId, salt, 0, authKeyData);

            var request = new PingRequest();

            var mTcpTransport = TcpTransportMock.Create();
            AddSendHandler(mTcpTransport, request);

            var recieveData = BinaryHelper.WriteBytes(new PongRequest(SendMessageId).SerializeBody);
            AddReceiveHandler(mTcpTransport, recieveData, request);

            this.RegisterMock(mTcpTransport);

            var mObjectPool = ObjectPoolMock.Create<ITcpTransport>().BuildPool(mTcpTransport);
            this.RegisterMock(mObjectPool);


            var mClientSettings = ClientSettingsMock.Create().AttachSession(() => mSession.Object);
            this.RegisterMock(mClientSettings);

            var mSessionStore = SessionStoreMock.Create();
            this.RegisterMock(mSessionStore);

            this.RegisterType<MtProtoSender>();

            // ---

            var mtProtoPlainSender = this.Resolve<MtProtoSender>();
            var sendTask = mtProtoPlainSender.SendAndRecive(request);

            // --

            Assert.Null(sendTask.Result);
            mTcpTransport.Verify(transport => transport.Send(It.IsAny<byte[]>()), Times.Once);
            mTcpTransport.Verify(transport => transport.Receieve(), Times.Once);

            mSessionStore.Verify(store => store.Save(It.IsAny<ISession>()), Times.Once);
            mSessionStore.Verify(store => store.Load(It.IsAny<string>()), Times.Never);
        }

        private void AddSendHandler(Mock<ITcpTransport> mock, TlMethod request)
        {
            mock
                .BuildSend(
                    bytes =>
                    {
                        var session = Container.Resolve<IClientSettings>().Session;

                        byte[] plainText = null;

                        BinaryHelper.ReadBytes(
                            bytes,
                            reader =>
                            {
                                Assert.Equal(session.AuthKey.Id, reader.ReadUInt64());
                                var msgKey = reader.ReadBytes(16);

                                var cipherText = reader.ReadBytes(bytes.Length - 8 - 16);

                                plainText = AES.DecryptAes(TlHelpers.CalcKey(session.AuthKey.Data, msgKey, true), cipherText);
                            });

                        Assert.NotNull(plainText);

                        BinaryHelper.ReadBytes(
                            plainText,
                            reader =>
                            {
                                Assert.Equal(session.Salt, reader.ReadUInt64());
                                Assert.Equal(session.Id, reader.ReadUInt64());
                                Assert.Equal(request.MessageId, reader.ReadInt64());
                                Assert.Equal(1, reader.ReadInt32());

                                var packetLength = reader.ReadInt32();
                                Assert.True(packetLength > 0);

                                var requestBytes = BinaryHelper.WriteBytes(request.SerializeBody);
                                Assert.Equal(requestBytes, reader.ReadBytes(packetLength));
                            });
                    });
        }

        private void AddReceiveHandler(Mock<ITcpTransport> mock, byte[] packet, TlMethod request)
        {
            mock.BuildReceieve(
                () =>
                {
                    var session = Container.Resolve<IClientSettings>().Session;
                    var plainTextStream = BinaryHelper.WriteBytes(
                        8 + 8 + 8 + 4 + 4 + packet.Length,
                        writer =>
                        {
                            writer.Write(session.Salt);
                            writer.Write(session.Id);
                            writer.Write(request.MessageId);
                            writer.Write(0);
                            writer.Write(packet.Length);
                            writer.Write(packet);
                        });

                    var plainText = plainTextStream.GetBytesWithBuffer();
                    plainTextStream.Dispose();

                    var msgKey = TlHelpers.CalcMsgKey(plainText);
                    var ciphertext = AES.EncryptAes(TlHelpers.CalcKey(session.AuthKey.Data, msgKey, false), plainText);

                    var chipedTextStream = BinaryHelper.WriteBytes(
                        8 + 16 + ciphertext.Length,
                        writer =>
                        {
                            writer.Write(session.AuthKey.Id);
                            writer.Write(msgKey);
                            writer.Write(ciphertext);
                        });
                    var result = chipedTextStream.ToArray();
                    chipedTextStream.Dispose();

                    return Task.FromResult(result);
                });
        }
    }
}
