using System.IO;
using TelegramClient.Entities.TL.Storage;

namespace TelegramClient.Entities.TL.Upload
{
    [TlObject(157948117)]
    public class TlFile : TlObject
    {
        public override int Constructor => 157948117;

        public TlAbsFileType Type { get; set; }
        public int Mtime { get; set; }
        public byte[] Bytes { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Type = (TlAbsFileType) ObjectUtils.DeserializeObject(br);
            Mtime = br.ReadInt32();
            Bytes = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Type, bw);
            bw.Write(Mtime);
            BytesUtil.Serialize(Bytes, bw);
        }
    }
}