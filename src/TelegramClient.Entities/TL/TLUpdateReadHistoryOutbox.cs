using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(791617983)]
    public class TlUpdateReadHistoryOutbox : TlAbsUpdate
    {
        public override int Constructor => 791617983;

        public TlAbsPeer Peer { get; set; }
        public int MaxId { get; set; }
        public int Pts { get; set; }
        public int PtsCount { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsPeer) ObjectUtils.DeserializeObject(br);
            MaxId = br.ReadInt32();
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MaxId);
            bw.Write(Pts);
            bw.Write(PtsCount);
        }
    }
}