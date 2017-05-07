namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;
    using System.IO.Compression;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class HandleGzipPackedRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HandleGzipPackedRecieveHandler));

        public uint[] HandleCodes { get; } = { 0x3072cfa1 };

        public byte[] HandleResponce(uint code, BinaryReader reader)
        {
            Log.Debug($"Recived Gzip message");

            using (var decompressStream = new MemoryStream())
            {
                using (var stream = new MemoryStream(Serializers.Bytes.Read(reader)))
                using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    zipStream.CopyTo(decompressStream);
                }

                decompressStream.Position = 0;
                return decompressStream.ToArray();
            }
        }
    }
}