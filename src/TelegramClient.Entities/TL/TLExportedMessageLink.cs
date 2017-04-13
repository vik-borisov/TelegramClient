using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(524838915)]
    public class TlExportedMessageLink : TlObject
    {
        public override int Constructor => 524838915;

        public string Link { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Link = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Link, bw);
        }
    }
}