using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(218751099)]
    public class TlInputPrivacyValueAllowContacts : TlAbsInputPrivacyRule
    {
        public override int Constructor => 218751099;


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