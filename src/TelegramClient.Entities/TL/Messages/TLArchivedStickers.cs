using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1338747336)]
    public class TlArchivedStickers : TlObject
    {
        public override int Constructor => 1338747336;

        public int Count { get; set; }
        public TlVector<TlAbsStickerSetCovered> Sets { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Sets = ObjectUtils.DeserializeVector<TlAbsStickerSetCovered>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Sets, bw);
        }
    }
}