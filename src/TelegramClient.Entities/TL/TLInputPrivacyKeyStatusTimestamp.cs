using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1335282456)]
    public class TlInputPrivacyKeyStatusTimestamp : TlAbsInputPrivacyKey
    {
        public override int Constructor => 1335282456;


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