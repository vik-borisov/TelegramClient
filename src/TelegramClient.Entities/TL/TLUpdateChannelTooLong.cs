using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-352032773)]
    public class TlUpdateChannelTooLong : TlAbsUpdate
    {
        public override int Constructor => -352032773;

        public int Flags { get; set; }
        public int ChannelId { get; set; }
        public int? Pts { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Pts != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            ChannelId = br.ReadInt32();
            if ((Flags & 1) != 0)
                Pts = br.ReadInt32();
            else
                Pts = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(ChannelId);
            if ((Flags & 1) != 0)
                bw.Write(Pts.Value);
        }
    }
}