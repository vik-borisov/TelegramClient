using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-906486552)]
    public class TlRequestSetPrivacy : TlMethod
    {
        public override int Constructor => -906486552;

        public TlAbsInputPrivacyKey Key { get; set; }
        public TlVector<TlAbsInputPrivacyRule> Rules { get; set; }
        public TlPrivacyRules Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Key = (TlAbsInputPrivacyKey) ObjectUtils.DeserializeObject(br);
            Rules = ObjectUtils.DeserializeVector<TlAbsInputPrivacyRule>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Key, bw);
            ObjectUtils.SerializeObject(Rules, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlPrivacyRules) ObjectUtils.DeserializeObject(br);
        }
    }
}