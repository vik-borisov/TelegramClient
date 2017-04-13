using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-346119674)]
    public class TlRequestSetInlineBotResults : TlMethod
    {
        public override int Constructor => -346119674;

        public int Flags { get; set; }
        public bool Gallery { get; set; }
        public bool Private { get; set; }
        public long QueryId { get; set; }
        public TlVector<TlAbsInputBotInlineResult> Results { get; set; }
        public int CacheTime { get; set; }
        public string NextOffset { get; set; }
        public TlInlineBotSwitchPm SwitchPm { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Gallery ? Flags | 1 : Flags & ~1;
            Flags = Private ? Flags | 2 : Flags & ~2;
            Flags = NextOffset != null ? Flags | 4 : Flags & ~4;
            Flags = SwitchPm != null ? Flags | 8 : Flags & ~8;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Gallery = (Flags & 1) != 0;
            Private = (Flags & 2) != 0;
            QueryId = br.ReadInt64();
            Results = ObjectUtils.DeserializeVector<TlAbsInputBotInlineResult>(br);
            CacheTime = br.ReadInt32();
            if ((Flags & 4) != 0)
                NextOffset = StringUtil.Deserialize(br);
            else
                NextOffset = null;

            if ((Flags & 8) != 0)
                SwitchPm = (TlInlineBotSwitchPm) ObjectUtils.DeserializeObject(br);
            else
                SwitchPm = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            bw.Write(QueryId);
            ObjectUtils.SerializeObject(Results, bw);
            bw.Write(CacheTime);
            if ((Flags & 4) != 0)
                StringUtil.Serialize(NextOffset, bw);
            if ((Flags & 8) != 0)
                ObjectUtils.SerializeObject(SwitchPm, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}