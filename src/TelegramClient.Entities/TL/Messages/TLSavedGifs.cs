using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(772213157)]
    public class TlSavedGifs : TlAbsSavedGifs
    {
        public override int Constructor => 772213157;

        public int Hash { get; set; }
        public TlVector<TlAbsDocument> Gifs { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt32();
            Gifs = ObjectUtils.DeserializeVector<TlAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Gifs, bw);
        }
    }
}