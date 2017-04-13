using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(1430961007)]
    public class TlPrivacyRules : TlObject
    {
        public override int Constructor => 1430961007;

        public TlVector<TlAbsPrivacyRule> Rules { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Rules = ObjectUtils.DeserializeVector<TlAbsPrivacyRule>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Rules, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}