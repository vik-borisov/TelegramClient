using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-613092008)]
    public class TlChatInvite : TlAbsChatInvite
    {
        public override int Constructor => -613092008;

        public int Flags { get; set; }
        public bool Channel { get; set; }
        public bool Broadcast { get; set; }
        public bool Public { get; set; }
        public bool Megagroup { get; set; }
        public string Title { get; set; }
        public TlAbsChatPhoto Photo { get; set; }
        public int ParticipantsCount { get; set; }
        public TlVector<TlAbsUser> Participants { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Channel ? Flags | 1 : Flags & ~1;
            Flags = Broadcast ? Flags | 2 : Flags & ~2;
            Flags = Public ? Flags | 4 : Flags & ~4;
            Flags = Megagroup ? Flags | 8 : Flags & ~8;
            Flags = Participants != null ? Flags | 16 : Flags & ~16;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Channel = (Flags & 1) != 0;
            Broadcast = (Flags & 2) != 0;
            Public = (Flags & 4) != 0;
            Megagroup = (Flags & 8) != 0;
            Title = StringUtil.Deserialize(br);
            Photo = (TlAbsChatPhoto) ObjectUtils.DeserializeObject(br);
            ParticipantsCount = br.ReadInt32();
            if ((Flags & 16) != 0)
                Participants = ObjectUtils.DeserializeVector<TlAbsUser>(br);
            else
                Participants = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            StringUtil.Serialize(Title, bw);
            ObjectUtils.SerializeObject(Photo, bw);
            bw.Write(ParticipantsCount);
            if ((Flags & 16) != 0)
                ObjectUtils.SerializeObject(Participants, bw);
        }
    }
}