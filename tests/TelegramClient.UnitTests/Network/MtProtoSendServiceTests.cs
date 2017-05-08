namespace TelegramClient.UnitTests.Network
{
    using System;
    using System.Threading.Tasks;

    using Autofac;

    using Moq;

    using TelegramClient.Core.Helpers;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Tcp;
    using TelegramClient.Core.Requests;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;
    using TelegramClient.Entities;
    using TelegramClient.UnitTests.Framework;

    using Xunit;


    public class MtProtoSendServiceTests : TestBase
    {

        [Fact]
        public async Task Send_SimpleCall_NotThrows()
        {
            const ulong SendMessageId = 1234;
            var authKey = SessionMock.GenerateAuthKeyData();
            ulong sessionId = 1231231;
            ulong salt = 5432111;
            var seqNo = 123;
            var mSession = SessionMock.Create()
                .BuildGenerateMessageSeqNo(confirm => Tuple.Create(SendMessageId, seqNo))
                .BuildSession(sessionId, salt, authKey);
            
            var request = new PingRequest();

            var mTcpTransport = TcpTransportMock.Create();
            AddSendHandler(mTcpTransport, request);

            this.RegisterMock(mTcpTransport);

            var mClientSettings = ClientSettingsMock.Create().AttachSession(() => mSession.Object);
            this.RegisterMock(mClientSettings);

            var mConfirmRecieve = ConfirmationRecieveServiceMock.Create()
                                                                .BuildWaitForConfirm(messageId =>
                                                                {
                                                                    Assert.Equal(SendMessageId, messageId);
                                                                    return Task.FromResult(true);
                                                                });
            this.RegisterMock(mConfirmRecieve);

            this.RegisterType<MtProtoSendService>();

            // ---

            var mtProtoPlainSender = this.Resolve<MtProtoSendService>();
            var sendResult =  mtProtoPlainSender.Send(request);

            await sendResult.Item1;
            // --

            mTcpTransport.Verify(transport => transport.Send(It.IsAny<byte[]>()), Times.Once);

            mConfirmRecieve.Verify(store => store.WaitForConfirm(It.IsAny<ulong>()), Times.Once);
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
                                Assert.Equal(0, reader.ReadInt32());

                                var packetLength = reader.ReadInt32();
                                Assert.True(packetLength > 0);

                                var requestBytes = BinaryHelper.WriteBytes(request.SerializeBody);
                                Assert.Equal(requestBytes, reader.ReadBytes(packetLength));
                            });
                    });
        }
    }
}
