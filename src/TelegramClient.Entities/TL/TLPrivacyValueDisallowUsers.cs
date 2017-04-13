using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(209668535)]
    public class TlPrivacyValueDisallowUsers : TlAbsPrivacyRule
    {
        public override int Constructor => 209668535;

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