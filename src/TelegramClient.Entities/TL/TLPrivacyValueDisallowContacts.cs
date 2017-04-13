using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-125240806)]
    public class TlPrivacyValueDisallowContacts : TlAbsPrivacyRule
    {
        public override int Constructor => -125240806;


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