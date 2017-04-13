using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1061556205)]
    public class TlChatParticipants : TlAbsChatParticipants
    {
        public override int Constructor => 1061556205;

        public int ChatId { get; set; }
        public TlVector<TlAbsChatParticipant> Participants { get; set; }
        public int Version { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            Participants = ObjectUtils.DeserializeVector<TlAbsChatParticipant>(br);
            Version = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            ObjectUtils.SerializeObject(Participants, bw);
            bw.Write(Version);
        }
    }
}