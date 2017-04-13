using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-634342611)]
    public class TlRequestImportContacts : TlMethod
    {
        public override int Constructor => -634342611;

        public TlVector<TlInputPhoneContact> Contacts { get; set; }
        public bool Replace { get; set; }
        public TlImportedContacts Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Contacts = ObjectUtils.DeserializeVector<TlInputPhoneContact>(br);
            Replace = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Contacts, bw);
            BoolUtil.Serialize(Replace, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlImportedContacts) ObjectUtils.DeserializeObject(br);
        }
    }
}