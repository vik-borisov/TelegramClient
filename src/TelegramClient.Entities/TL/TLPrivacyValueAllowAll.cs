using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1698855810)]
    public class TlPrivacyValueAllowAll : TlAbsPrivacyRule
    {
        public override int Constructor => 1698855810;


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