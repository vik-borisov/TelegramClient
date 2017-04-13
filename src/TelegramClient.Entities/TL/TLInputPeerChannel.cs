using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(548253432)]
    public class TlInputPeerChannel : TlAbsInputPeer
    {
        public override int Constructor => 548253432;

        public int ChannelId { get; set; }
        public long AccessHash { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt32();
            AccessHash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
            bw.Write(AccessHash);
        }
    }
}