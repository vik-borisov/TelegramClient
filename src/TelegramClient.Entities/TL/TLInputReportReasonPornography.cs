using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(777640226)]
    public class TlInputReportReasonPornography : TlAbsReportReason
    {
        public override int Constructor => 777640226;


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