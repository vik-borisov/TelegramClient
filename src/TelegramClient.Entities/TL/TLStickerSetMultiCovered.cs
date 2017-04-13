using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(872932635)]
    public class TlStickerSetMultiCovered : TlAbsStickerSetCovered
    {
        public override int Constructor => 872932635;

        public TlStickerSet Set { get; set; }
        public TlVector<TlAbsDocument> Covers { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Set = (TlStickerSet) ObjectUtils.DeserializeObject(br);
            Covers = ObjectUtils.DeserializeVector<TlAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Set, bw);
            ObjectUtils.SerializeObject(Covers, bw);
        }
    }
}