using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-298113238)]
    public class TlUpdatePrivacy : TlAbsUpdate
    {
        public override int Constructor => -298113238;

        public TlAbsPrivacyKey Key { get; set; }
        public TlVector<TlAbsPrivacyRule> Rules { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Key = (TlAbsPrivacyKey) ObjectUtils.DeserializeObject(br);
            Rules = ObjectUtils.DeserializeVector<TlAbsPrivacyRule>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Key, bw);
            ObjectUtils.SerializeObject(Rules, bw);
        }
    }
}