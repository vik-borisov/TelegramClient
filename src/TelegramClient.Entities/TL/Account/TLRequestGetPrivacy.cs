using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-623130288)]
    public class TlRequestGetPrivacy : TlMethod
    {
        public override int Constructor => -623130288;

        public TlAbsInputPrivacyKey Key { get; set; }
        public TlPrivacyRules Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Key = (TlAbsInputPrivacyKey) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Key, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlPrivacyRules) ObjectUtils.DeserializeObject(br);
        }
    }
}