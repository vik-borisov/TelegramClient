using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1725551049)]
    public class TlChannelMessages : TlAbsMessages
    {
        public override int Constructor => -1725551049;

        public int Flags { get; set; }
        public int Pts { get; set; }
        public int Count { get; set; }
        public TlVector<TlAbsMessage> Messages { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Pts = br.ReadInt32();
            Count = br.ReadInt32();
            Messages = ObjectUtils.DeserializeVector<TlAbsMessage>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Pts);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Messages, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}