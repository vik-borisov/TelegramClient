using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-57668565)]
    public class TlChatParticipantsForbidden : TlAbsChatParticipants
    {
        public override int Constructor => -57668565;

        public int Flags { get; set; }
        public int ChatId { get; set; }
        public TlAbsChatParticipant SelfParticipant { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = SelfParticipant != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            ChatId = br.ReadInt32();
            if ((Flags & 1) != 0)
                SelfParticipant = (TlAbsChatParticipant) ObjectUtils.DeserializeObject(br);
            else
                SelfParticipant = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(ChatId);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(SelfParticipant, bw);
        }
    }
}