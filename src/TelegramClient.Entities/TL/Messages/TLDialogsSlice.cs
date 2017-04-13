using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1910543603)]
    public class TlDialogsSlice : TlAbsDialogs
    {
        public override int Constructor => 1910543603;

        public int Count { get; set; }
        public TlVector<TlDialog> Dialogs { get; set; }
        public TlVector<TlAbsMessage> Messages { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Dialogs = ObjectUtils.DeserializeVector<TlDialog>(br);
            Messages = ObjectUtils.DeserializeVector<TlAbsMessage>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Dialogs, bw);
            ObjectUtils.SerializeObject(Messages, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}