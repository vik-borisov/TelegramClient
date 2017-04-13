using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1336546578)]
    public class TlMessageActionChannelMigrateFrom : TlAbsMessageAction
    {
        public override int Constructor => -1336546578;

        public string Title { get; set; }
        public int ChatId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Title = StringUtil.Deserialize(br);
            ChatId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Title, bw);
            bw.Write(ChatId);
        }
    }
}