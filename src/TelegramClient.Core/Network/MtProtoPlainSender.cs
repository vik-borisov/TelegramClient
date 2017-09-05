using System.IO;
using System.Threading.Tasks;

namespace TelegramClient.Core.Network
{
    using log4net;

    using TelegramClient.Core.Helpers;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Tcp;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(IMtProtoPlainSender))]
    internal class MtProtoPlainSender : IMtProtoPlainSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoPlainSender));

        public ITcpTransport TcpTransport { get; set; }

        public IClientSettings ClientSettings { get; set; }

        private byte[] PrepareToSend(byte[] data)
        {
            var newMessageId = ClientSettings.Session.GenerateMsgId();
            Log.Debug($"Send message with id : {newMessageId}");

            return BinaryHelper.WriteBytes(
                writer =>
                {
                    writer.Write((long)0);
                    writer.Write(newMessageId);
                    writer.Write(data.Length);
                    writer.Write(data);
                });
        }

        public async Task<byte[]> SendAndReceive(byte[] data)
        {
            var preparedPacket = PrepareToSend(data);

            await TcpTransport.Send(preparedPacket).ConfigureAwait(false);

            var result = await TcpTransport.Receieve().ConfigureAwait(false);

            return ProcessReceivedMessage(result);
        }

        private byte[] ProcessReceivedMessage(byte[] recievedMessage)
        {
            using (var memoryStream = new MemoryStream(recievedMessage))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                var authKeyid = binaryReader.ReadInt64();
                var messageId = binaryReader.ReadInt64();
                var messageLength = binaryReader.ReadInt32();

                Log.Debug($"Recieve message with id : {messageId}");

                var response = binaryReader.ReadBytes(messageLength);

                return response;
            }
        }
    }
}