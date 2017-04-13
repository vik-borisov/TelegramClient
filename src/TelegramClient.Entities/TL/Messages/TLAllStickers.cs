using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-302170017)]
    public class TlAllStickers : TlAbsAllStickers
    {
        public override int Constructor => -302170017;

        public int Hash { get; set; }
        public TlVector<TlStickerSet> Sets { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt32();
            Sets = ObjectUtils.DeserializeVector<TlStickerSet>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Sets, bw);
        }
    }
}