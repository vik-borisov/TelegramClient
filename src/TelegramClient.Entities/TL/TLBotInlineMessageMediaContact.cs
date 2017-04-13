using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(904770772)]
    public class TlBotInlineMessageMediaContact : TlAbsBotInlineMessage
    {
        public override int Constructor => 904770772;

        public int Flags { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            PhoneNumber = StringUtil.Deserialize(br);
            FirstName = StringUtil.Deserialize(br);
            LastName = StringUtil.Deserialize(br);
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
            StringUtil.Serialize(PhoneNumber, bw);
            StringUtil.Serialize(FirstName, bw);
            StringUtil.Serialize(LastName, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
        }
    }
}