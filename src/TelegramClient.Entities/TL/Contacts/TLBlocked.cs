using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(471043349)]
    public class TlBlocked : TlAbsBlocked
    {
        public override int Constructor => 471043349;

        public TlVector<TlContactBlocked> Blocked { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Blocked = ObjectUtils.DeserializeVector<TlContactBlocked>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Blocked, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}