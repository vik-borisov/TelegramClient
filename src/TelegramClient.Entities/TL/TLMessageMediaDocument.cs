using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-203411800)]
    public class TlMessageMediaDocument : TlAbsMessageMedia
    {
        public override int Constructor => -203411800;

        public TlAbsDocument Document { get; set; }
        public string Caption { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Document = (TlAbsDocument) ObjectUtils.DeserializeObject(br);
            Caption = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Document, bw);
            StringUtil.Serialize(Caption, bw);
        }
    }
}