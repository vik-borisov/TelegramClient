using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-512463606)]
    public class TlInputReportReasonOther : TlAbsReportReason
    {
        public override int Constructor => -512463606;

        public string Text { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Text = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Text, bw);
        }
    }
}