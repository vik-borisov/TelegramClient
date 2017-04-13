using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1558317424)]
    public class TlRecentStickers : TlAbsRecentStickers
    {
        public override int Constructor => 1558317424;

        public int Hash { get; set; }
        public TlVector<TlAbsDocument> Stickers { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt32();
            Stickers = ObjectUtils.DeserializeVector<TlAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Stickers, bw);
        }
    }
}