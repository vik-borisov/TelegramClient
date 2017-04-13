using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(297109817)]
    public class TlDocumentAttributeAnimated : TlAbsDocumentAttribute
    {
        public override int Constructor => 297109817;


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