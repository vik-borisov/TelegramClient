using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-400399203)]
    public class TlRequestGetGameHighScores : TlMethod
    {
        public override int Constructor => -400399203;

        public TlAbsInputPeer Peer { get; set; }
        public int Id { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlHighScores Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Id = br.ReadInt32();
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(Id);
            ObjectUtils.SerializeObject(UserId, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlHighScores) ObjectUtils.DeserializeObject(br);
        }
    }
}