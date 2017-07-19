namespace TelegramClient.UnitTests.Network
{
    using System.IO;
    using System.Threading;

    using Autofac;

    using Moq;

    using OpenTl.Schema;

    using TelegramClient.Core.Helpers;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network.Recieve;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;
    using TelegramClient.UnitTests.Framework;

    using Xunit;


    public class RecieveServiceTests : TestBase
    {

        [Fact]
        public void Recieve_SimpleCall_NotThrows()
        {
            const ulong RequestMessageId = 1234;
            var authKeyData = SessionMock.GenerateAuthKeyData();
            const ulong sessionId = 123456;
            const ulong salt = 654321;
            uint[] rpcResponceCode = {0xf35c6d01};

            var sendUser = new TDialog
            {
                ReadInboxMaxId = 132,
                Peer = new TPeerUser { UserId = 123 },
                NotifySettings = new TPeerNotifySettingsEmpty(),
                Draft = new TDraftMessageEmpty()
            };

            var mSession = SessionMock.Create().BuildSession(sessionId, salt, authKeyData);
            this.RegisterMock(mSession);

            var mClientSettings = ClientSettingsMock.Create().AttachSession(() => mSession.Object);
            this.RegisterMock(mClientSettings);

            var mTcpTransport = TcpTransportMock.Create().BuildReceieve(out var tsc);
            this.RegisterMock(mTcpTransport);

            var mConfrimSendService = ConfirmationSendServiceMock.Create().BuildAddForSend(messageId => Assert.Equal(RequestMessageId, messageId));
            this.RegisterMock(mConfrimSendService);

            var mRecieveHandler = RecieveHandlerMock.Create().BuildRecieveHandler(rpcResponceCode).BuildHandleResponce(
                (code, reader) =>
                {
                    Assert.Equal(RequestMessageId, reader.ReadUInt64());
                    return  null;
                });
            this.RegisterMock(mRecieveHandler);
            this.RegisterAdapterForHandler();

            this.RegisterType<RecievingService>();

            // ---
            var recieveData = EncodePacket(BinaryHelper.WriteBytes(new RpcResponce(RequestMessageId, sendUser).Serialize), RequestMessageId);

            var mtProtoPlainSender = this.Resolve<RecievingService>();
            mtProtoPlainSender.StartReceiving();
            Thread.Sleep(500);

            tsc.SetResult(recieveData);

            Thread.Sleep(500);

            // --
            mRecieveHandler.Verify(recieveService => recieveService.HandleResponce(It.IsAny<uint>(), It.IsAny<BinaryReader>()), Times.Once);
            mConfrimSendService.Verify(recieveService => recieveService.AddForSend(It.IsAny<long>()), Times.Once);
        }

        private byte[] EncodePacket(byte[] packet, ulong messageId)
        {
            var session = Container.Resolve<IClientSettings>().Session;
            var plainTextStream = BinaryHelper.WriteBytes(
                8 + 8 + 8 + 4 + 4 + packet.Length,
                writer =>
                {
                    writer.Write(session.Salt);
                    writer.Write(session.Id);
                    writer.Write(messageId);
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

            return result;
        }
    }
}
