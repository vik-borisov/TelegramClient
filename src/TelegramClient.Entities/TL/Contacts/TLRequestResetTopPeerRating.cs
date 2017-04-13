using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(451113900)]
    public class TlRequestResetTopPeerRating : TlMethod
    {
        public override int Constructor => 451113900;

        public TlAbsTopPeerCategory Category { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Category = (TlAbsTopPeerCategory) ObjectUtils.DeserializeObject(br);
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Category, bw);
            ObjectUtils.SerializeObject(Peer, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}