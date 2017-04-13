using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(1041346555)]
    public class TlChannelDifferenceEmpty : TlAbsChannelDifference
    {
        public override int Constructor => 1041346555;

        public int Flags { get; set; }
        public bool Final { get; set; }
        public int Pts { get; set; }
        public int? Timeout { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Final ? Flags | 1 : Flags & ~1;
            Flags = Timeout != null ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Final = (Flags & 1) != 0;
            Pts = br.ReadInt32();
            if ((Flags & 2) != 0)
                Timeout = br.ReadInt32();
            else
                Timeout = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            bw.Write(Pts);
            if ((Flags & 2) != 0)
                bw.Write(Timeout.Value);
        }
    }
}