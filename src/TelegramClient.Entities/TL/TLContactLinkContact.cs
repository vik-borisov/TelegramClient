using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-721239344)]
    public class TlContactLinkContact : TlAbsContactLink
    {
        public override int Constructor => -721239344;


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