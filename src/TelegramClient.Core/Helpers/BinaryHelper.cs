namespace TelegramClient.Core.Helpers
{
    using System;
    using System.IO;
    using System.Text;

    internal static class BinaryHelper
    {
        public static void ReadBytes(byte[] bytes, Action<BinaryReader> action)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    action(binaryReader);
                }
            }
        }

        public static byte[] WriteBytes(Action<BinaryWriter> action)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    action(binaryWriter);
                }

                return memoryStream.ToArray();
            }
        }

        public static MemoryStream WriteBytes(int bufferSize, Action<BinaryWriter> action)
        {
            var memoryStream = new MemoryStream(new byte[bufferSize], 0, bufferSize, true, true);
            using (var binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true))
            {
                action(binaryWriter);
            }

            return memoryStream;
        }

        public static byte[] GetBytesWithBuffer(this MemoryStream stream)
        {
            var success = stream.TryGetBuffer(out var buffer);
            return buffer.Array;
        }
    }
}