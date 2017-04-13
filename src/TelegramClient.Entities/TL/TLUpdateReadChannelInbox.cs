using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1108669311)]
    public class TlUpdateReadChannelInbox : TlAbsUpdate
    {
        public override int Constructor => 1108669311;

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