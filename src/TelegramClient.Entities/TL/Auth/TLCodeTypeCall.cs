using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1948046307)]
    public class TlCodeTypeCall : TlAbsCodeType
    {
        public override int Constructor => 1948046307;


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