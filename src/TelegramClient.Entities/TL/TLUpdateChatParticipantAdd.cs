using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-364179876)]
    public class TlUpdateChatParticipantAdd : TlAbsUpdate
    {
        public override int Constructor => -364179876;

        public int ChatId { get; set; }
        public int UserId { get; set; }
        public int InviterId { get; set; }
        public int Date { get; set; }
        public int Version { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = br.ReadInt32();
            InviterId = br.ReadInt32();
            Date = br.ReadInt32();
            Version = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            bw.Write(UserId);
            bw.Write(InviterId);
            bw.Write(Date);
            bw.Write(Version);
        }
    }
}