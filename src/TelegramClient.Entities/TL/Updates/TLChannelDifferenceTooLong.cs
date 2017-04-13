using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(1091431943)]
    public class TlChannelDifferenceTooLong : TlAbsChannelDifference
    {
        public override int Constructor => 1091431943;

        public int Flags { get; set; }
        public bool Final { get; set; }
        public int Pts { get; set; }
        public int? Timeout { get; set; }
        public int TopMessage { get; set; }
        public int ReadInboxMaxId { get; set; }
        public int ReadOutboxMaxId { get; set; }
        public int UnreadCount { get; set; }
        public TlVector<TlAbsMessage> Messages { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Final ? Flags | 1 : Flags & ~1;
            Flags = Timeout != null ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Final = (Flags & 1) != 0;
            Pts = br.ReadInt32();
            if ((Flags & 2) != 0)
                Timeout = br.ReadInt32();
            else
                Timeout = null;

            TopMessage = br.ReadInt32();
            ReadInboxMaxId = br.ReadInt32();
            ReadOutboxMaxId = br.ReadInt32();
            UnreadCount = br.ReadInt32();
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
            if ((Flags & 2) != 0)
                bw.Write(Timeout.Value);
            bw.Write(TopMessage);
            bw.Write(ReadInboxMaxId);
            bw.Write(ReadOutboxMaxId);
            bw.Write(UnreadCount);
            ObjectUtils.SerializeObject(Messages, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}