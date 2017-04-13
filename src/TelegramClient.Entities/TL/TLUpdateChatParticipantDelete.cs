using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1851755554)]
    public class TlUpdateChatParticipantDelete : TlAbsUpdate
    {
        public override int Constructor => 1851755554;

        public int ChatId { get; set; }
        public int UserId { get; set; }
        public int Version { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = br.ReadInt32();
            Version = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            bw.Write(UserId);
            bw.Write(Version);
        }
    }
}