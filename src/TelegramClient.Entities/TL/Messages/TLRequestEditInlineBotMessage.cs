using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(319564933)]
    public class TlRequestEditInlineBotMessage : TlMethod
    {
        public override int Constructor => 319564933;

        public int Flags { get; set; }
        public bool NoWebpage { get; set; }
        public TlInputBotInlineMessageId Id { get; set; }
        public string Message { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }
        public TlVector<TlAbsMessageEntity> Entities { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = NoWebpage ? Flags | 2 : Flags & ~2;
            Flags = Message != null ? Flags | 2048 : Flags & ~2048;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
            Flags = Entities != null ? Flags | 8 : Flags & ~8;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            NoWebpage = (Flags & 2) != 0;
            Id = (TlInputBotInlineMessageId) ObjectUtils.DeserializeObject(br);
            if ((Flags & 2048) != 0)
                Message = StringUtil.Deserialize(br);
            else
                Message = null;

            if ((Flags & 4) != 0)
                ReplyMarkup = (TlAbsReplyMarkup) ObjectUtils.DeserializeObject(br);
            else
                ReplyMarkup = null;

            if ((Flags & 8) != 0)
                Entities = ObjectUtils.DeserializeVector<TlAbsMessageEntity>(br);
            else
                Entities = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Id, bw);
            if ((Flags & 2048) != 0)
                StringUtil.Serialize(Message, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
            if ((Flags & 8) != 0)
                ObjectUtils.SerializeObject(Entities, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}