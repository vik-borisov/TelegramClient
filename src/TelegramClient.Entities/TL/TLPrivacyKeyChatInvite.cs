using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1343122938)]
    public class TlPrivacyKeyChatInvite : TlAbsPrivacyKey
    {
        public override int Constructor => 1343122938;


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