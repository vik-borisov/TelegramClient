using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1262639204)]
    public class TlInputBotInlineMessageGame : TlAbsInputBotInlineMessage
    {
        public override int Constructor => 1262639204;

        public int Flags { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
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
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
        }
    }
}