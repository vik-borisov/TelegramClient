using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1888354709)]
    public class TlRequestForwardMessages : TlMethod
    {
        public override int Constructor => 1888354709;

        public int Flags { get; set; }
        public bool Silent { get; set; }
        public bool Background { get; set; }
        public bool WithMyScore { get; set; }
        public TlAbsInputPeer FromPeer { get; set; }
        public TlVector<int> Id { get; set; }
        public TlVector<long> RandomId { get; set; }
        public TlAbsInputPeer ToPeer { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Silent ? Flags | 32 : Flags & ~32;
            Flags = Background ? Flags | 64 : Flags & ~64;
            Flags = WithMyScore ? Flags | 256 : Flags & ~256;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Silent = (Flags & 32) != 0;
            Background = (Flags & 64) != 0;
            WithMyScore = (Flags & 256) != 0;
            FromPeer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Id = ObjectUtils.DeserializeVector<int>(br);
            RandomId = ObjectUtils.DeserializeVector<long>(br);
            ToPeer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            ObjectUtils.SerializeObject(FromPeer, bw);
            ObjectUtils.SerializeObject(Id, bw);
            ObjectUtils.SerializeObject(RandomId, bw);
            ObjectUtils.SerializeObject(ToPeer, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}