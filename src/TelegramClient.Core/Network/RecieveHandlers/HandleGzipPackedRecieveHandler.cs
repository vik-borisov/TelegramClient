namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.MTProto;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    internal class HandleGzipPackedRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HandleGzipPackedRecieveHandler));

        public uint ResponceCode { get; } = 0x3072cfa1;

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
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
                yield return decompressStream.ToArray();
            }
        }
    }
}