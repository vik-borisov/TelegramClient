using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1937807902)]
    public class TlBotInlineMessageText : TlAbsBotInlineMessage
    {
        public override int Constructor => -1937807902;

        public int Flags { get; set; }
        public bool NoWebpage { get; set; }
        public string Message { get; set; }
        public TlVector<TlAbsMessageEntity> Entities { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = NoWebpage ? Flags | 1 : Flags & ~1;
            Flags = Entities != null ? Flags | 2 : Flags & ~2;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            NoWebpage = (Flags & 1) != 0;
            Message = StringUtil.Deserialize(br);
            if ((Flags & 2) != 0)
                Entities = ObjectUtils.DeserializeVector<TlAbsMessageEntity>(br);
            else
                Entities = null;

            if ((Flags & 4) != 0)
                ReplyMarkup = (TlAbsReplyMarkup) ObjectUtils.DeserializeObject(br);
            else
                ReplyMarkup = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            StringUtil.Serialize(Message, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(Entities, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
        }
    }
}