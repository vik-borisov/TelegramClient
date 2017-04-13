using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2009052699)]
    public class TlPhotoSize : TlAbsPhotoSize
    {
        public override int Constructor => 2009052699;

        public string Type { get; set; }
        public TlAbsFileLocation Location { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int Size { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Type = StringUtil.Deserialize(br);
            Location = (TlAbsFileLocation) ObjectUtils.DeserializeObject(br);
            W = br.ReadInt32();
            H = br.ReadInt32();
            Size = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Type, bw);
            ObjectUtils.SerializeObject(Location, bw);
            bw.Write(W);
            bw.Write(H);
            bw.Write(Size);
        }
    }
}