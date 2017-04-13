using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(423314455)]
    public class TlInputNotifyUsers : TlAbsInputNotifyPeer
    {
        public override int Constructor => 423314455;


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