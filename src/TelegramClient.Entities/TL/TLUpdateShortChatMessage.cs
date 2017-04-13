using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(377562760)]
    public class TlUpdateShortChatMessage : TlAbsUpdates
    {
        public override int Constructor => 377562760;

        public int Flags { get; set; }
        public bool Out { get; set; }
        public bool Mentioned { get; set; }
        public bool MediaUnread { get; set; }
        public bool Silent { get; set; }
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ChatId { get; set; }
        public string Message { get; set; }
        public int Pts { get; set; }
        public int PtsCount { get; set; }
        public int Date { get; set; }
        public TlMessageFwdHeader FwdFrom { get; set; }
        public int? ViaBotId { get; set; }
        public int? ReplyToMsgId { get; set; }
        public TlVector<TlAbsMessageEntity> Entities { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Out ? Flags | 2 : Flags & ~2;
            Flags = Mentioned ? Flags | 16 : Flags & ~16;
            Flags = MediaUnread ? Flags | 32 : Flags & ~32;
            Flags = Silent ? Flags | 8192 : Flags & ~8192;
            Flags = FwdFrom != null ? Flags | 4 : Flags & ~4;
            Flags = ViaBotId != null ? Flags | 2048 : Flags & ~2048;
            Flags = ReplyToMsgId != null ? Flags | 8 : Flags & ~8;
            Flags = Entities != null ? Flags | 128 : Flags & ~128;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Out = (Flags & 2) != 0;
            Mentioned = (Flags & 16) != 0;
            MediaUnread = (Flags & 32) != 0;
            Silent = (Flags & 8192) != 0;
            Id = br.ReadInt32();
            FromId = br.ReadInt32();
            ChatId = br.ReadInt32();
            Message = StringUtil.Deserialize(br);
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
            Date = br.ReadInt32();
            if ((Flags & 4) != 0)
                FwdFrom = (TlMessageFwdHeader) ObjectUtils.DeserializeObject(br);
            else
                FwdFrom = null;

            if ((Flags & 2048) != 0)
                ViaBotId = br.ReadInt32();
            else
                ViaBotId = null;

            if ((Flags & 8) != 0)
                ReplyToMsgId = br.ReadInt32();
            else
                ReplyToMsgId = null;

            if ((Flags & 128) != 0)
                Entities = ObjectUtils.DeserializeVector<TlAbsMessageEntity>(br);
            else
                Entities = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            bw.Write(Id);
            bw.Write(FromId);
            bw.Write(ChatId);
            StringUtil.Serialize(Message, bw);
            bw.Write(Pts);
            bw.Write(PtsCount);
            bw.Write(Date);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(FwdFrom, bw);
            if ((Flags & 2048) != 0)
                bw.Write(ViaBotId.Value);
            if ((Flags & 8) != 0)
                bw.Write(ReplyToMsgId.Value);
            if ((Flags & 128) != 0)
                ObjectUtils.SerializeObject(Entities, bw);
        }
    }
}