using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(646922073)]
    public class TlContactLinkHasPhone : TlAbsContactLink
    {
        public override int Constructor => 646922073;


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