using System;
using System.IO;
using System.Threading.Tasks;

namespace TelegramClient.Core.Network
{
    using CodeProject.ObjectPool;

    using log4net;

    using TelegramClient.Core.Settings;

    internal class MtProtoPlainSender : IMtProtoPlainSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoPlainSender));

        public IObjectPool<PooledObjectWrapper<ITcpTransport>> TcpTransportPool { get; set; }

        public IClientSettings ClientSettings { get; set; }

        private async Task Send(ITcpTransport tcpTransport, byte[] data)
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

                    var packet = memoryStream.ToArray();

                    await tcpTransport.Send(packet);
                }
            }
        }

        public async Task<byte[]> SendAndReceive(byte[] data)
        {
            using (var wrapper = TcpTransportPool.GetObject())
            {
                Log.Debug($"Use TcpTransport instance : {wrapper.PooledObjectInfo.Id}");

                await Send(wrapper.InternalResource, data);
                return await Receive(wrapper.InternalResource);
            }
        }

        private async Task<byte[]> Receive(ITcpTransport tcpTransport)
        {
            var result = await tcpTransport.Receieve();

            using (var memoryStream = new MemoryStream(result.Body))
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