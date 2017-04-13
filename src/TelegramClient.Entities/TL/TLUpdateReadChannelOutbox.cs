using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(634833351)]
    public class TlUpdateReadChannelOutbox : TlAbsUpdate
    {
        public override int Constructor => 634833351;

        public int ChannelId { get; set; }
        public int MaxId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt32();
            MaxId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
            bw.Write(MaxId);
        }
    }
}