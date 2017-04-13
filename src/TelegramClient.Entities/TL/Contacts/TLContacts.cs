using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(1871416498)]
    public class TlContacts : TlAbsContacts
    {
        public override int Constructor => 1871416498;

        public TlVector<TlContact> Contacts { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Contacts = ObjectUtils.DeserializeVector<TlContact>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Contacts, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}