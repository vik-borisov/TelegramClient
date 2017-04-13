using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1109531342)]
    public class TlPeerChannel : TlAbsPeer
    {
        public override int Constructor => -1109531342;

        public int ChannelId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
        }
    }
}