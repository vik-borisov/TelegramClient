using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-299124375)]
    public class TlUpdateDraftMessage : TlAbsUpdate
    {
        public override int Constructor => -299124375;

        public TlAbsPeer Peer { get; set; }
        public TlAbsDraftMessage Draft { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsPeer) ObjectUtils.DeserializeObject(br);
            Draft = (TlAbsDraftMessage) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(Draft, bw);
        }
    }
}