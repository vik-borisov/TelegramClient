namespace TelegramClient.Core.Network.Send
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using OpenTl.Schema;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve.Interfaces;
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

        public ISessionStore SessionStore { get; set; }

        public IResponseResultGetter ResponseResultGetter { get; set; }
        
        public async Task<long> Send(IObject obj, CancellationToken cancellationToken)
        {
            Log.Debug($"Send object {obj}");

            (byte[] preparedData, long mesId) = await PrepareToSend(obj).ConfigureAwait(false);

            await TcpTransport.Send(preparedData, cancellationToken).ConfigureAwait(false);

            await SessionStore.Save().ConfigureAwait(false);

            return mesId;
        }
        
        public async Task<Task<object>> SendAndWaitResponse(IObject obj, CancellationToken cancellationToken)
        {
            Log.Debug($"Send object and wait for a response {obj}");

            (byte[] preparedData, long mesId) = await PrepareToSend(obj).ConfigureAwait(false);

            var responseTask = ResponseResultGetter.Receive(mesId, cancellationToken);
            
            await TcpTransport.Send(preparedData, cancellationToken).ConfigureAwait(false);

            await SessionStore.Save().ConfigureAwait(false);

            return responseTask;
        }

        private static MemoryStream MakeMemory(int len)
        {
            return new MemoryStream(new byte[len], 0, len, true, true);
        }

        private async Task<(byte[], long)> PrepareToSend(IObject obj)
        {
            var packet = Serializer.SerializeObject(obj);

            (long mesId, int seqNo) = await ClientSettings.Session.GenerateMsgIdAndSeqNo(obj is IRequest).ConfigureAwait(false);

            Log.Debug($"Send message with Id = {mesId} and seqNo = {seqNo}");

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
                    plaintextWriter.Write(seqNo);
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

                    return (ciphertextPacket.ToArray(), mesId);
                }
            }
        }
    }
}