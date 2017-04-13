using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-1219778094)]
    public class TlContactsNotModified : TlAbsContacts
    {
        public override int Constructor => -1219778094;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}