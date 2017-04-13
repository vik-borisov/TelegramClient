using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1240849242)]
    public class TlStickerSet : TlObject
    {
        public override int Constructor => -1240849242;

        public TlStickerSet Set { get; set; }
        public TlVector<TlStickerPack> Packs { get; set; }
        public TlVector<TlAbsDocument> Documents { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Set = (TlStickerSet) ObjectUtils.DeserializeObject(br);
            Packs = ObjectUtils.DeserializeVector<TlStickerPack>(br);
            Documents = ObjectUtils.DeserializeVector<TlAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Set, bw);
            ObjectUtils.SerializeObject(Packs, bw);
            ObjectUtils.SerializeObject(Documents, bw);
        }
    }
}