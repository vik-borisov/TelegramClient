using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1923290508)]
    public class TlCodeTypeSms : TlAbsCodeType
    {
        public override int Constructor => 1923290508;


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