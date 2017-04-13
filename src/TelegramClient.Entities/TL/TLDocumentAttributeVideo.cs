using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1494273227)]
    public class TlDocumentAttributeVideo : TlAbsDocumentAttribute
    {
        public override int Constructor => 1494273227;

        public int Duration { get; set; }
        public int W { get; set; }
        public int H { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Duration = br.ReadInt32();
            W = br.ReadInt32();
            H = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Duration);
            bw.Write(W);
            bw.Write(H);
        }
    }
}