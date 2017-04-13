using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1959820384)]
    public class TlNotifyAll : TlAbsNotifyPeer
    {
        public override int Constructor => 1959820384;


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