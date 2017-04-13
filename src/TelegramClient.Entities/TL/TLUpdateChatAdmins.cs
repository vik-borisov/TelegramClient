using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1855224129)]
    public class TlUpdateChatAdmins : TlAbsUpdate
    {
        public override int Constructor => 1855224129;

        public int ChatId { get; set; }
        public bool Enabled { get; set; }
        public int Version { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            Enabled = BoolUtil.Deserialize(br);
            Version = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            BoolUtil.Serialize(Enabled, bw);
            bw.Write(Version);
        }
    }
}