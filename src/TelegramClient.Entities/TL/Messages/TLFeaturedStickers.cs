using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-123893531)]
    public class TlFeaturedStickers : TlAbsFeaturedStickers
    {
        public override int Constructor => -123893531;

        public int Hash { get; set; }
        public TlVector<TlAbsStickerSetCovered> Sets { get; set; }
        public TlVector<long> Unread { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt32();
            Sets = ObjectUtils.DeserializeVector<TlAbsStickerSetCovered>(br);
            Unread = ObjectUtils.DeserializeVector<long>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Sets, bw);
            ObjectUtils.SerializeObject(Unread, bw);
        }
    }
}