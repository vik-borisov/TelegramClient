using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1744710921)]
    public class TlDocumentAttributeHasStickers : TlAbsDocumentAttribute
    {
        public override int Constructor => -1744710921;


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