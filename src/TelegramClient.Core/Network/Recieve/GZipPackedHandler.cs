namespace TelegramClient.Core.Network.Recieve
{
    using System.IO;
    using System.IO.Compression;

    using log4net;

    using OpenTl.Schema;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Recieve.Interfaces;

    [SingleInstance(typeof(IGZipPackedHandler))]
    internal class GZipPackedHandler : IGZipPackedHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GZipPackedHandler));

        public IObject HandleGZipPacked(TgZipPacked obj)
        {
            Log.Debug($"Recived Gzip message");

            using (var decompressStream = new MemoryStream())
            {
                using (var stream = new MemoryStream(obj.PackedData))
                using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    zipStream.CopyTo(decompressStream);
                }

                decompressStream.Position = 0;

                using (var reader = new BinaryReader(decompressStream))
                {
                    return Serializer.DeserializeObject(reader);
                }
            }
        }
    }
}