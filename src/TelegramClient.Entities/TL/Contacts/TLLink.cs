using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(986597452)]
    public class TlLink : TlObject
    {
        public override int Constructor => 986597452;

        public TlAbsContactLink MyLink { get; set; }
        public TlAbsContactLink ForeignLink { get; set; }
        public TlAbsUser User { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            MyLink = (TlAbsContactLink) ObjectUtils.DeserializeObject(br);
            ForeignLink = (TlAbsContactLink) ObjectUtils.DeserializeObject(br);
            User = (TlAbsUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(MyLink, bw);
            ObjectUtils.SerializeObject(ForeignLink, bw);
            ObjectUtils.SerializeObject(User, bw);
        }
    }
}