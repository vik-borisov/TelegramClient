using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1261946036)]
    public class TlNotifyUsers : TlAbsNotifyPeer
    {
        public override int Constructor => -1261946036;


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