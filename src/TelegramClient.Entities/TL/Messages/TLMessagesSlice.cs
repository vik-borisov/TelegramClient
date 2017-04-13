using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(189033187)]
    public class TlMessagesSlice : TlAbsMessages
    {
        public override int Constructor => 189033187;

        public int Count { get; set; }
        public TlVector<TlAbsMessage> Messages { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Messages = ObjectUtils.DeserializeVector<TlAbsMessage>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Messages, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}