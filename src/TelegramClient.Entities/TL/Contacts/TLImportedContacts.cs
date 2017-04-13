using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-1387117803)]
    public class TlImportedContacts : TlObject
    {
        public override int Constructor => -1387117803;

        public TlVector<TlImportedContact> Imported { get; set; }
        public TlVector<long> RetryContacts { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Imported = ObjectUtils.DeserializeVector<TlImportedContact>(br);
            RetryContacts = ObjectUtils.DeserializeVector<long>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Imported, bw);
            ObjectUtils.SerializeObject(RetryContacts, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}