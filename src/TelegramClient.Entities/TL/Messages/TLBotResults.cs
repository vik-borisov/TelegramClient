using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(627509670)]
    public class TlBotResults : TlObject
    {
        public override int Constructor => 627509670;

        public int Flags { get; set; }
        public bool Gallery { get; set; }
        public long QueryId { get; set; }
        public string NextOffset { get; set; }
        public TlInlineBotSwitchPm SwitchPm { get; set; }
        public TlVector<TlAbsBotInlineResult> Results { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Gallery ? Flags | 1 : Flags & ~1;
            Flags = NextOffset != null ? Flags | 2 : Flags & ~2;
            Flags = SwitchPm != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Gallery = (Flags & 1) != 0;
            QueryId = br.ReadInt64();
            if ((Flags & 2) != 0)
                NextOffset = StringUtil.Deserialize(br);
            else
                NextOffset = null;

            if ((Flags & 4) != 0)
                SwitchPm = (TlInlineBotSwitchPm) ObjectUtils.DeserializeObject(br);
            else
                SwitchPm = null;

            Results = ObjectUtils.DeserializeVector<TlAbsBotInlineResult>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            bw.Write(QueryId);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(NextOffset, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(SwitchPm, bw);
            ObjectUtils.SerializeObject(Results, bw);
        }
    }
}