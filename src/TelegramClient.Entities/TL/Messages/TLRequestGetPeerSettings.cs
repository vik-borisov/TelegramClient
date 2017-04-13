using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(913498268)]
    public class TlRequestGetPeerSettings : TlMethod
    {
        public override int Constructor => 913498268;

        public TlAbsInputPeer Peer { get; set; }
        public TlPeerSettings Response { get; set; }


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

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlPeerSettings) ObjectUtils.DeserializeObject(br);
        }
    }
}