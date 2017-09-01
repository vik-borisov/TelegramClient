namespace TelegramClient.Core.Helpers
{
    using System;
    using System.IO;

    internal static class BinaryHelper
    {
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
    }
}