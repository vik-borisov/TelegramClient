using System.IO;
using TelegramClient.Entities.TL.Contacts;

namespace TelegramClient.Entities.TL
{
    [TlObject(1496513539)]
    public class TlUserFull : TlObject
    {
        public override int Constructor => 1496513539;

        public int Flags { get; set; }
        public bool Blocked { get; set; }
        public TlAbsUser User { get; set; }
        public string About { get; set; }
        public TlLink Link { get; set; }
        public TlAbsPhoto ProfilePhoto { get; set; }
        public TlAbsPeerNotifySettings NotifySettings { get; set; }
        public TlBotInfo BotInfo { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Blocked ? Flags | 1 : Flags & ~1;
            Flags = About != null ? Flags | 2 : Flags & ~2;
            Flags = ProfilePhoto != null ? Flags | 4 : Flags & ~4;
            Flags = BotInfo != null ? Flags | 8 : Flags & ~8;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Blocked = (Flags & 1) != 0;
            User = (TlAbsUser) ObjectUtils.DeserializeObject(br);
            if ((Flags & 2) != 0)
                About = StringUtil.Deserialize(br);
            else
                About = null;

            Link = (TlLink) ObjectUtils.DeserializeObject(br);
            if ((Flags & 4) != 0)
                ProfilePhoto = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
            else
                ProfilePhoto = null;

            NotifySettings = (TlAbsPeerNotifySettings) ObjectUtils.DeserializeObject(br);
            if ((Flags & 8) != 0)
                BotInfo = (TlBotInfo) ObjectUtils.DeserializeObject(br);
            else
                BotInfo = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(User, bw);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(About, bw);
            ObjectUtils.SerializeObject(Link, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ProfilePhoto, bw);
            ObjectUtils.SerializeObject(NotifySettings, bw);
            if ((Flags & 8) != 0)
                ObjectUtils.SerializeObject(BotInfo, bw);
        }
    }
}