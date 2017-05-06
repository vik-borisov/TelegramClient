//namespace TelegramClient.UnitTests.Network
//{
//    using System.Threading.Tasks;

//    using Autofac;

//    using Moq;

//    using TelegramClient.Core.Helpers;
//    using TelegramClient.Core.MTProto.Crypto;
//    using TelegramClient.Core.Network.Recieve;
//    using TelegramClient.Core.Responces;
//    using TelegramClient.Core.Settings;
//    using TelegramClient.Core.Utils;
//    using TelegramClient.Entities.TL;
//    using TelegramClient.UnitTests.Framework;

//    using Xunit;


//    public class MtProtoRecieveServiceTests : TestBase
//    {

//        [Fact]
//        public async Task Recieve_SimpleCall_NotThrows()
//        {
//            const ulong RequestMessageId = 1234;
//            var authKeyData = SessionMock.GenerateAuthKeyData();
//            const ulong sessionId = 123456;
//            const ulong salt = 654321;

//            var sendUser = new TlDialog
//                           {
//                               ReadInboxMaxId = 132,
//                               Peer = new TlPeerUser{UserId = 123},
//                               NotifySettings = new TlPeerNotifySettingsEmpty(),
//                               Draft = new TlDraftMessageEmpty()
//                           };
//            var rpcResponce = new RpcResponce(RequestMessageId, sendUser);

//            var mSession = SessionMock.Create().BuildSession(sessionId, salt, authKeyData);
//            this.RegisterMock(mSession);

//            var mClientSettings = ClientSettingsMock.Create().AttachSession(() => mSession.Object);
//            this.RegisterMock(mClientSettings);

//            var tsc = new TaskCompletionSource<byte[]>();
//            var mTcpTransport = TcpTransportMock.Create().BuildReceieve(() => tsc.Task);
//            this.RegisterMock(mTcpTransport);

//            var mConfrimRecieveService = ConfirmationRecieveServiceMock.Create().BuildConfirmRequest(messageId => Assert.Equal(RequestMessageId, messageId));
//            this.RegisterMock(mConfrimRecieveService);

//            var mConfrimSendService = ConfirmationSendServiceMock.Create().BuildAddForSend(messageId => Assert.Equal(RequestMessageId, messageId));
//            this.RegisterMock(mConfrimSendService);

//            this.RegisterType<RecievingService>();

//            // ---

//            var mtProtoPlainSender = this.Resolve<RecievingService>();
//            mtProtoPlainSender.StartReceiving();

//            var recieveTask = mtProtoPlainSender.Recieve(RequestMessageId);

       
//            var recieveData = EncodePacket(BinaryHelper.WriteBytes(rpcResponce.Serialize), RequestMessageId);
//            tsc.SetResult(recieveData);

//            recieveTask.Wait();

//            mtProtoPlainSender.StopRecieving();

//            var recieveUser = new TlDialog();
//            recieveUser.Deserialize(recieveTask.Result);

//            // --

//            Assert.Equal(sendUser.ReadInboxMaxId, recieveUser.ReadInboxMaxId);
//            Assert.Equal(((TlPeerUser)sendUser.Peer).UserId, ((TlPeerUser)recieveUser.Peer).UserId);

//            mConfrimRecieveService.Verify(recieveService => recieveService.ConfirmRequest(It.IsAny<ulong>()), Times.Once);
//            mConfrimSendService.Verify(recieveService => recieveService.AddForSend(It.IsAny<ulong>()), Times.Once);
//        }

//        private byte[] EncodePacket(byte[] packet, ulong messageId)
//        {
//            var session = Container.Resolve<IClientSettings>().Session;
//            var plainTextStream = BinaryHelper.WriteBytes(
//                8 + 8 + 8 + 4 + 4 + packet.Length,
//                writer =>
//                {
//                    writer.Write(session.Salt);
//                    writer.Write(session.Id);
//                    writer.Write(messageId);
//                    writer.Write(0);
//                    writer.Write(packet.Length);
//                    writer.Write(packet);
//                });

//            var plainText = plainTextStream.GetBytesWithBuffer();
//            plainTextStream.Dispose();

//            var msgKey = TlHelpers.CalcMsgKey(plainText);
//            var ciphertext = AES.EncryptAes(TlHelpers.CalcKey(session.AuthKey.Data, msgKey, false), plainText);

//            var chipedTextStream = BinaryHelper.WriteBytes(
//                8 + 16 + ciphertext.Length,
//                writer =>
//                {
//                    writer.Write(session.AuthKey.Id);
//                    writer.Write(msgKey);
//                    writer.Write(ciphertext);
//                });
//            var result = chipedTextStream.ToArray();
//            chipedTextStream.Dispose();

//            return result;
//        }
//    }
//}
