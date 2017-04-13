using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1540769658)]
    public class TlInputNotifyAll : TlAbsInputNotifyPeer
    {
        public override int Constructor => -1540769658;


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