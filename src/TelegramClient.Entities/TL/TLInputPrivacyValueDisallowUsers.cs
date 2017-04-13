using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1877932953)]
    public class TlInputPrivacyValueDisallowUsers : TlAbsInputPrivacyRule
    {
        public override int Constructor => -1877932953;

        public TlVector<TlAbsInputUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Users = ObjectUtils.DeserializeVector<TlAbsInputUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}