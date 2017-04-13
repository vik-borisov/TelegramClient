using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1137792208)]
    public class TlPrivacyKeyStatusTimestamp : TlAbsPrivacyKey
    {
        public override int Constructor => -1137792208;


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