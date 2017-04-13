using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1955338397)]
    public class TlPrivacyValueDisallowAll : TlAbsPrivacyRule
    {
        public override int Constructor => -1955338397;


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