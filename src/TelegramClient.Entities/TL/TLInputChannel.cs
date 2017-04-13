using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1343524562)]
    public class TlInputChannel : TlAbsInputChannel
    {
        public override int Constructor => -1343524562;

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