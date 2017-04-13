using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1032643901)]
    public class TlMessageMediaPhoto : TlAbsMessageMedia
    {
        public override int Constructor => 1032643901;

        public TlAbsPhoto Photo { get; set; }
        public string Caption { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Photo = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
            Caption = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Photo, bw);
            StringUtil.Serialize(Caption, bw);
        }
    }
}