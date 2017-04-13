using System.IO;

namespace TelegramClient.Entities.TL
{
    [TLObject(2107670217)]
    public class TLInputPeerSelf : TLAbsInputPeer
    {
        public override int Constructor => 2107670217;


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