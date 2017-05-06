using System.IO;
using System.Text;

namespace TelegramClient.Entities
{
    public class BytesUtil
    {
        private static byte[] Read(BinaryReader binaryReader)
        {
            var firstByte = binaryReader.ReadByte();
            int len, padding;
            if (firstByte == 254)
            {
                len = binaryReader.ReadByte() | (binaryReader.ReadByte() << 8) | (binaryReader.ReadByte() << 16);
                padding = len % 4;
            }
            else
            {
                len = firstByte;
                padding = (len + 1) % 4;
            }

            var data = binaryReader.ReadBytes(len);
            if (padding > 0)
            {
                padding = 4 - padding;
                binaryReader.ReadBytes(padding);
            }

            return data;
        }

        private static BinaryWriter Write(BinaryWriter binaryWriter, byte[] data)
        {
            int padding;
            if (data.Length < 254)
            {
                padding = (data.Length + 1) % 4;
                if (padding != 0)
                    padding = 4 - padding;

                binaryWriter.Write((byte) data.Length);
                binaryWriter.Write(data);
            }
            else
            {
                padding = data.Length % 4;
                if (padding != 0)
                    padding = 4 - padding;

                binaryWriter.Write((byte) 254);
                binaryWriter.Write((byte) data.Length);
                binaryWriter.Write((byte) (data.Length >> 8));
                binaryWriter.Write((byte) (data.Length >> 16));
                binaryWriter.Write(data);
            }


            for (var i = 0; i < padding; i++)
                binaryWriter.Write((byte) 0);

            return binaryWriter;
        }

        public static byte[] Deserialize(BinaryReader reader)
        {
            return Read(reader);
        }

        public static void Serialize(byte[] src, BinaryWriter writer)
        {
            Write(writer, src);
        }
    }

    public class StringUtil
    {
        public static string Deserialize(BinaryReader reader)
        {
            var data = BytesUtil.Deserialize(reader);
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static void Serialize(string src, BinaryWriter writer)
        {
            BytesUtil.Serialize(Encoding.UTF8.GetBytes(src), writer);
        }
    }

    public class BoolUtil
    {
        public static bool Deserialize(BinaryReader reader)
        {
            var falseCNumber = -1132882121;
            var trueCNumber = -1720552011;
            var readed = reader.ReadInt32();
            if (readed == falseCNumber) return false;
            if (readed == trueCNumber) return true;
            throw new InvalidDataException(string.Format("Invalid Boolean Data : {0}", readed));
        }

        public static void Serialize(bool src, BinaryWriter writer)
        {
            var falseCNumber = -1132882121;
            var trueCNumber = -1720552011;
            writer.Write(src ? trueCNumber : falseCNumber);
        }
    }
}