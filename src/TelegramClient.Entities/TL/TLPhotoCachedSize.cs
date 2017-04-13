using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-374917894)]
    public class TlPhotoCachedSize : TlAbsPhotoSize
    {
        public override int Constructor => -374917894;

        public string Type { get; set; }
        public TlAbsFileLocation Location { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public byte[] Bytes { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Type = StringUtil.Deserialize(br);
            Location = (TlAbsFileLocation) ObjectUtils.DeserializeObject(br);
            W = br.ReadInt32();
            H = br.ReadInt32();
            Bytes = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Type, bw);
            ObjectUtils.SerializeObject(Location, bw);
            bw.Write(W);
            bw.Write(H);
            BytesUtil.Serialize(Bytes, bw);
        }
    }
}