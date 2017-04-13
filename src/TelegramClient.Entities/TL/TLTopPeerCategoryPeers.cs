using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-75283823)]
    public class TlTopPeerCategoryPeers : TlObject
    {
        public override int Constructor => -75283823;

        public TlAbsTopPeerCategory Category { get; set; }
        public int Count { get; set; }
        public TlVector<TlTopPeer> Peers { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Category = (TlAbsTopPeerCategory) ObjectUtils.DeserializeObject(br);
            Count = br.ReadInt32();
            Peers = ObjectUtils.DeserializeVector<TlTopPeer>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Category, bw);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Peers, bw);
        }
    }
}