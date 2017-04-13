using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(764901049)]
    public class TlRequestGetPeerDialogs : TlMethod
    {
        public override int Constructor => 764901049;

        public TlVector<TlAbsInputPeer> Peers { get; set; }
        public TlPeerDialogs Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peers = ObjectUtils.DeserializeVector<TlAbsInputPeer>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peers, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlPeerDialogs) ObjectUtils.DeserializeObject(br);
        }
    }
}