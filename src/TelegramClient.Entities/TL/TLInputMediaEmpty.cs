using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1771768449)]
    public class TlInputMediaEmpty : TlAbsInputMedia
    {
        public override int Constructor => -1771768449;


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