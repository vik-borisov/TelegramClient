using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-39416522)]
    public class TlRequestGetMessageEditData : TlMethod
    {
        public override int Constructor => -39416522;

        public TlAbsInputPeer Peer { get; set; }
        public int Id { get; set; }
        public TlMessageEditData Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Id = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(Id);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlMessageEditData) ObjectUtils.DeserializeObject(br);
        }
    }
}