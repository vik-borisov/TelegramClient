using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2134579434)]
    public class TlInputPeerEmpty : TlAbsInputPeer
    {
        public override int Constructor => 2134579434;


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