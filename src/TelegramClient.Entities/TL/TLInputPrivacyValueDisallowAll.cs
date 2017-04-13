using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-697604407)]
    public class TlInputPrivacyValueDisallowAll : TlAbsInputPrivacyRule
    {
        public override int Constructor => -697604407;


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