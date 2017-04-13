using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1195615476)]
    public class TlInputNotifyPeer : TlAbsInputNotifyPeer
    {
        public override int Constructor => -1195615476;

        public TlAbsInputPeer Peer { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
        }
    }
}