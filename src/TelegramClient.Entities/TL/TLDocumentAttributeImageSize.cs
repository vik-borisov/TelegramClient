using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1815593308)]
    public class TlDocumentAttributeImageSize : TlAbsDocumentAttribute
    {
        public override int Constructor => 1815593308;

        public int W { get; set; }
        public int H { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            W = br.ReadInt32();
            H = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(W);
            bw.Write(H);
        }
    }
}