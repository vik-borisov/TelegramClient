using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1587647177)]
    public class TlRequestGetRecentStickers : TlMethod
    {
        public override int Constructor => 1587647177;

        public int Flags { get; set; }
        public bool Attached { get; set; }
        public int Hash { get; set; }
        public TlAbsRecentStickers Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Attached ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Attached = (Flags & 1) != 0;
            Hash = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsRecentStickers) ObjectUtils.DeserializeObject(br);
        }
    }
}