using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1158290442)]
    public class TlFoundGifs : TlObject
    {
        public override int Constructor => 1158290442;

        public int NextOffset { get; set; }
        public TlVector<TlAbsFoundGif> Results { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            NextOffset = br.ReadInt32();
            Results = ObjectUtils.DeserializeVector<TlAbsFoundGif>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(NextOffset);
            ObjectUtils.SerializeObject(Results, bw);
        }
    }
}