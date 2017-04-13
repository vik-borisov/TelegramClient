using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1896289088)]
    public class TlRequestSetGameScore : TlMethod
    {
        public override int Constructor => -1896289088;

        public int Flags { get; set; }
        public bool EditMessage { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public int Id { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public int Score { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = EditMessage ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            EditMessage = (Flags & 1) != 0;
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Id = br.ReadInt32();
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Score = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(Id);
            ObjectUtils.SerializeObject(UserId, bw);
            bw.Write(Score);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}