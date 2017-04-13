using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(125178264)]
    public class TlUpdateChatParticipants : TlAbsUpdate
    {
        public override int Constructor => 125178264;

        public TlAbsChatParticipants Participants { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Participants = (TlAbsChatParticipants) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Participants, bw);
        }
    }
}