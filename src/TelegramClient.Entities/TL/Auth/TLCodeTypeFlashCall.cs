using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(577556219)]
    public class TlCodeTypeFlashCall : TlAbsCodeType
    {
        public override int Constructor => 577556219;


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