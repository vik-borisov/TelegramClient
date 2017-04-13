using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-993483427)]
    public class TlRequestGetMessagesViews : TlMethod
    {
        public override int Constructor => -993483427;

        public TlAbsInputPeer Peer { get; set; }
        public TlVector<int> Id { get; set; }
        public bool Increment { get; set; }
        public TlVector<int> Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Id = ObjectUtils.DeserializeVector<int>(br);
            Increment = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(Id, bw);
            BoolUtil.Serialize(Increment, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = ObjectUtils.DeserializeVector<int>(br);
        }
    }
}