using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(407582158)]
    public class TlInputPrivacyValueAllowAll : TlAbsInputPrivacyRule
    {
        public override int Constructor => 407582158;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}