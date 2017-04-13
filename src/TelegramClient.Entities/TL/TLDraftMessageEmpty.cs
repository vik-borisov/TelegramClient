using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1169445179)]
    public class TlDraftMessageEmpty : TlAbsDraftMessage
    {
        public override int Constructor => -1169445179;


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