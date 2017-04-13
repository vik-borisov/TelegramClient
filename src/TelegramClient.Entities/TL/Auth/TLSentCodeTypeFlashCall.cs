using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-1425815847)]
    public class TlSentCodeTypeFlashCall : TlAbsSentCodeType
    {
        public override int Constructor => -1425815847;

        public string Pattern { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Pattern = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Pattern, bw);
        }
    }
}