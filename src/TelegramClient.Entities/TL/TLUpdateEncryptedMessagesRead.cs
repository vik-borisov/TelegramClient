using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(956179895)]
    public class TlUpdateEncryptedMessagesRead : TlAbsUpdate
    {
        public override int Constructor => 956179895;

        public int ChatId { get; set; }
        public int MaxDate { get; set; }
        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            MaxDate = br.ReadInt32();
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            bw.Write(MaxDate);
            bw.Write(Date);
        }
    }
}