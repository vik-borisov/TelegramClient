using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1232070311)]
    public class TlUpdateChatParticipantAdmin : TlAbsUpdate
    {
        public override int Constructor => -1232070311;

        public int ChatId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public int Version { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = br.ReadInt32();
            IsAdmin = BoolUtil.Deserialize(br);
            Version = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            bw.Write(UserId);
            BoolUtil.Serialize(IsAdmin, bw);
            bw.Write(Version);
        }
    }
}