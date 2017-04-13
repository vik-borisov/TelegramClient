using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-636267638)]
    public class TlChatParticipantCreator : TlAbsChatParticipant
    {
        public override int Constructor => -636267638;

        public int UserId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
        }
    }
}