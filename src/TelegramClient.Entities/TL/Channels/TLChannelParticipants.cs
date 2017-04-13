using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-177282392)]
    public class TlChannelParticipants : TlObject
    {
        public override int Constructor => -177282392;

        public int Count { get; set; }
        public TlVector<TlAbsChannelParticipant> Participants { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Participants = ObjectUtils.DeserializeVector<TlAbsChannelParticipant>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Participants, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}