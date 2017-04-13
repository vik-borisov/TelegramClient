using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-305282981)]
    public class TlTopPeer : TlObject
    {
        public override int Constructor => -305282981;

        public TlAbsPeer Peer { get; set; }
        public double Rating { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsPeer) ObjectUtils.DeserializeObject(br);
            Rating = br.ReadDouble();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(Rating);
        }
    }
}