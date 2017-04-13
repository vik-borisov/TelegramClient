using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1218642516)]
    public class TlReplyInlineMarkup : TlAbsReplyMarkup
    {
        public override int Constructor => 1218642516;

        public TlVector<TlKeyboardButtonRow> Rows { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Rows = ObjectUtils.DeserializeVector<TlKeyboardButtonRow>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Rows, bw);
        }
    }
}