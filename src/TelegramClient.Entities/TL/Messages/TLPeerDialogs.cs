using System.IO;
using TelegramClient.Entities.TL.Updates;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(863093588)]
    public class TlPeerDialogs : TlObject
    {
        public override int Constructor => 863093588;

        public TlVector<TlDialog> Dialogs { get; set; }
        public TlVector<TlAbsMessage> Messages { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }
        public TlState State { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Dialogs = ObjectUtils.DeserializeVector<TlDialog>(br);
            Messages = ObjectUtils.DeserializeVector<TlAbsMessage>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
            State = (TlState) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Dialogs, bw);
            ObjectUtils.SerializeObject(Messages, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
            ObjectUtils.SerializeObject(State, bw);
        }
    }
}