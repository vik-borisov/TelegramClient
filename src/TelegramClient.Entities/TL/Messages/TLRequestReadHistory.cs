using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(238054714)]
    public class TlRequestReadHistory : TlMethod
    {
        public override int Constructor => 238054714;

        public TlAbsInputPeer Peer { get; set; }
        public int MaxId { get; set; }
        public TlAffectedMessages Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            MaxId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MaxId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAffectedMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}