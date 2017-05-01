using System.IO;
using System.Threading.Tasks;

namespace TelegramClient.Core.Network
{
    using log4net;

    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Settings;

    internal class MtProtoPlainSender : IMtProtoPlainSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoPlainSender));

        public ITcpTransport TcpTransport { get; set; }

        public IClientSettings ClientSettings { get; set; }

        private byte[] PrepareToSend(byte[] data)
        {
            var newMessageId = ClientSettings.Session.GetNewMessageId();
            Log.Debug($"Send message with id : {newMessageId}");

            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write((long) 0);
                    binaryWriter.Write(newMessageId);
                    binaryWriter.Write(data.Length);
                    binaryWriter.Write(data);

                 return memoryStream.ToArray();
                }
            }
        }

        public async Task<byte[]> SendAndReceive(byte[] data)
        {
            var preparedPacket = PrepareToSend(data);

            TcpTransport.Send(preparedPacket);
            var result = await TcpTransport.Receieve();

            return ProcessReceivedMessage(result);
        }

        private byte[] ProcessReceivedMessage(TcpMessage recievedMessage)
        {
            using (var memoryStream = new MemoryStream(recievedMessage.Body))
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