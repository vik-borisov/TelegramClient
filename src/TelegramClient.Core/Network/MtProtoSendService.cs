namespace TelegramClient.Core.Network
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using log4net;

    using OpenTl.Schema;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Tcp;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;

    [SingleInstance(typeof(IMtProtoSender))]
    internal class MtProtoSendService : IMtProtoSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoSendService));

        public ITcpTransport TcpTransport { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public ISessionStore SessionStore { get; set; }

        public async Task<Tuple<Task, long>> Send(IObject obj)
        {
            var preparedData = PrepareToSend(obj, out var mesId);

            await TcpTransport.Send(preparedData).ConfigureAwait(false);

            SessionStore.Save();

            var waitTask = ConfirmationRecieveService.WaitForConfirm(mesId);

            return Tuple.Create(waitTask, mesId);
        }

        private MemoryStream MakeMemory(int len)
        {
            return new MemoryStream(new byte[len], 0, len, true, true);
        }

        private byte[] PrepareToSend(IObject obj, out long mesId)
        {
            var packet = Serializer.SerializeObject(obj);

            var genResult = ClientSettings.Session.GenerateMsgIdAndSeqNo(obj is IRequest);
            mesId = genResult.Item1;

            Log.Debug($"Send message with Id = {mesId} and seqNo = {genResult.Item2}");

            byte[] msgKey;
            byte[] ciphertext;
            var randomPaddingLenght = TlHelpers.GenerateRandomInt(1024 / 16) * 16;

            using (var plaintextPacket = MakeMemory(8 + 8 + 8 + 4 + 4 + packet.Length + randomPaddingLenght))
            {
                using (var plaintextWriter = new BinaryWriter(plaintextPacket))
                {
                    plaintextWriter.Write(ClientSettings.Session.Salt);
                    plaintextWriter.Write(ClientSettings.Session.Id);
                    plaintextWriter.Write(mesId);
                    plaintextWriter.Write(genResult.Item2);
                    plaintextWriter.Write(packet.Length);
                    plaintextWriter.Write(packet);

                    plaintextWriter.Write(TlHelpers.GenerateRandomBytes(randomPaddingLenght));

                    plaintextPacket.TryGetBuffer(out var buffer);

                    var authKey = ClientSettings.Session.AuthKey.Data;
                    msgKey = TlHelpers.CalcMsgKey(authKey, buffer.Array);

                    var key = TlHelpers.CalcKey(authKey, msgKey, true);

                    ciphertext = AES.EncryptAes(key, buffer.Array);
                }
            }

            using (var ciphertextPacket = MakeMemory(8 + 16 + ciphertext.Length))
            {
                using (var writer = new BinaryWriter(ciphertextPacket))
                {
                    writer.Write(ClientSettings.Session.AuthKey.Id);
                    writer.Write(msgKey);
                    writer.Write(ciphertext);

                    return ciphertextPacket.ToArray();
                }
            }
        }
    }
}