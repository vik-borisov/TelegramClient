using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1704596961)]
    public class TlUpdateChatUserTyping : TlAbsUpdate
    {
        public override int Constructor => -1704596961;

        public int ChatId { get; set; }
        public int UserId { get; set; }
        public TlAbsSendMessageAction Action { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = br.ReadInt32();
            Action = (TlAbsSendMessageAction) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            bw.Write(UserId);
            ObjectUtils.SerializeObject(Action, bw);
        }
    }
}