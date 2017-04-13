using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1613493288)]
    public class TlNotifyPeer : TlAbsNotifyPeer
    {
        public override int Constructor => -1613493288;

        public TlAbsPeer Peer { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsPeer) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
        }
    }
}