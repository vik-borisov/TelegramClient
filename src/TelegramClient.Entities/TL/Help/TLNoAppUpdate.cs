using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-1000708810)]
    public class TlNoAppUpdate : TlAbsAppUpdate
    {
        public override int Constructor => -1000708810;


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