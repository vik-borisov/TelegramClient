using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(505595789)]
    public class TlInputReportReasonViolence : TlAbsReportReason
    {
        public override int Constructor => 505595789;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}