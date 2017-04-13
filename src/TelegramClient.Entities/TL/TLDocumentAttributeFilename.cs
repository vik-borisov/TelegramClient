using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(358154344)]
    public class TlDocumentAttributeFilename : TlAbsDocumentAttribute
    {
        public override int Constructor => 358154344;

        public string FileName { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            FileName = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(FileName, bw);
        }
    }
}