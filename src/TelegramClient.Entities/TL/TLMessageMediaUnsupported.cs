using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1618676578)]
    public class TlMessageMediaUnsupported : TlAbsMessageMedia
    {
        public override int Constructor => -1618676578;


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