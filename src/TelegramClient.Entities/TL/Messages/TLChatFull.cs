using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-438840932)]
    public class TlChatFull : TlObject
    {
        public override int Constructor => -438840932;

        public TlAbsChatFull FullChat { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            FullChat = (TlAbsChatFull) ObjectUtils.DeserializeObject(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(FullChat, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}