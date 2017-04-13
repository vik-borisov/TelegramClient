using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1678812626)]
    public class TlStickerSetCovered : TlAbsStickerSetCovered
    {
        public override int Constructor => 1678812626;

        public TlStickerSet Set { get; set; }
        public TlAbsDocument Cover { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Set = (TlStickerSet) ObjectUtils.DeserializeObject(br);
            Cover = (TlAbsDocument) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Set, bw);
            ObjectUtils.SerializeObject(Cover, bw);
        }
    }
}