using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(543450958)]
    public class TlChannelDifference : TlAbsChannelDifference
    {
        public override int Constructor => 543450958;

        public int Flags { get; set; }
        public bool Final { get; set; }
        public int Pts { get; set; }
        public int? Timeout { get; set; }
        public TlVector<TlAbsMessage> NewMessages { get; set; }
        public TlVector<TlAbsUpdate> OtherUpdates { get; set; }
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

            NewMessages = ObjectUtils.DeserializeVector<TlAbsMessage>(br);
            OtherUpdates = ObjectUtils.DeserializeVector<TlAbsUpdate>(br);
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
            ObjectUtils.SerializeObject(NewMessages, bw);
            ObjectUtils.SerializeObject(OtherUpdates, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}