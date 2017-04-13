using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1227598250)]
    public class TlUpdateChannel : TlAbsUpdate
    {
        public override int Constructor => -1227598250;

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