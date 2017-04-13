using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1574314746)]
    public class TlUpdateConfig : TlAbsUpdate
    {
        public override int Constructor => -1574314746;


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