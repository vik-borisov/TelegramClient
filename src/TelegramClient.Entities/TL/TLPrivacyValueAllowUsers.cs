using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1297858060)]
    public class TlPrivacyValueAllowUsers : TlAbsPrivacyRule
    {
        public override int Constructor => 1297858060;

        public TlVector<int> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Users = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}