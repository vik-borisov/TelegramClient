using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-947462709)]
    public class TlMessageFwdHeader : TlObject
    {
        public override int Constructor => -947462709;

        public int Flags { get; set; }
        public int? FromId { get; set; }
        public int Date { get; set; }
        public int? ChannelId { get; set; }
        public int? ChannelPost { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = FromId != null ? Flags | 1 : Flags & ~1;
            Flags = ChannelId != null ? Flags | 2 : Flags & ~2;
            Flags = ChannelPost != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            if ((Flags & 1) != 0)
                FromId = br.ReadInt32();
            else
                FromId = null;

            Date = br.ReadInt32();
            if ((Flags & 2) != 0)
                ChannelId = br.ReadInt32();
            else
                ChannelId = null;

            if ((Flags & 4) != 0)
                ChannelPost = br.ReadInt32();
            else
                ChannelPost = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            if ((Flags & 1) != 0)
                bw.Write(FromId.Value);
            bw.Write(Date);
            if ((Flags & 2) != 0)
                bw.Write(ChannelId.Value);
            if ((Flags & 4) != 0)
                bw.Write(ChannelPost.Value);
        }
    }
}