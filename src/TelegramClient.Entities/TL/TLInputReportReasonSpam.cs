using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1490799288)]
    public class TlInputReportReasonSpam : TlAbsReportReason
    {
        public override int Constructor => 1490799288;


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