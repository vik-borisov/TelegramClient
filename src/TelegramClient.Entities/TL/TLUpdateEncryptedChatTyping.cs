using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(386986326)]
    public class TlUpdateEncryptedChatTyping : TlAbsUpdate
    {
        public override int Constructor => 386986326;

        public int ChatId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
        }
    }
}