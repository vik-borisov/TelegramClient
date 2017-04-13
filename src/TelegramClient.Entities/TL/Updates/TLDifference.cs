using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(16030880)]
    public class TlDifference : TlAbsDifference
    {
        public override int Constructor => 16030880;

        public TlVector<TlAbsMessage> NewMessages { get; set; }
        public TlVector<TlAbsEncryptedMessage> NewEncryptedMessages { get; set; }
        public TlVector<TlAbsUpdate> OtherUpdates { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }
        public TlState State { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            NewMessages = ObjectUtils.DeserializeVector<TlAbsMessage>(br);
            NewEncryptedMessages = ObjectUtils.DeserializeVector<TlAbsEncryptedMessage>(br);
            OtherUpdates = ObjectUtils.DeserializeVector<TlAbsUpdate>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
            State = (TlState) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(NewMessages, bw);
            ObjectUtils.SerializeObject(NewEncryptedMessages, bw);
            ObjectUtils.SerializeObject(OtherUpdates, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
            ObjectUtils.SerializeObject(State, bw);
        }
    }
}