using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-123988)]
    public class TlPrivacyValueAllowContacts : TlAbsPrivacyRule
    {
        public override int Constructor => -123988;


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