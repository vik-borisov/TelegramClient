using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(865483769)]
    public class TlRequestForwardMessage : TlMethod
    {
        public override int Constructor => 865483769;

        public TlAbsInputPeer Peer { get; set; }
        public int Id { get; set; }
        public long RandomId { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Id = br.ReadInt32();
            RandomId = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(Id);
            bw.Write(RandomId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}