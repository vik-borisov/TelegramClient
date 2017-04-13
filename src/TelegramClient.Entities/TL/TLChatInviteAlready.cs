using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1516793212)]
    public class TlChatInviteAlready : TlAbsChatInvite
    {
        public override int Constructor => 1516793212;

        public TlAbsChat Chat { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Chat = (TlAbsChat) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Chat, bw);
        }
    }
}