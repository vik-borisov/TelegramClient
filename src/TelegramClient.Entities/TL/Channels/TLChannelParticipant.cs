using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-791039645)]
    public class TlChannelParticipant : TlObject
    {
        public override int Constructor => -791039645;

        public TlAbsChannelParticipant Participant { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Participant = (TlAbsChannelParticipant) ObjectUtils.DeserializeObject(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Participant, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}