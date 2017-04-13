using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(691006739)]
    public class TlInputBotInlineMessageMediaAuto : TlAbsInputBotInlineMessage
    {
        public override int Constructor => 691006739;

        public int Flags { get; set; }
        public string Caption { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Caption = StringUtil.Deserialize(br);
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
            StringUtil.Serialize(Caption, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
        }
    }
}