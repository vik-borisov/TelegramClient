using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-1126886015)]
    public class TlRequestSignIn : TlMethod
    {
        public override int Constructor => -1126886015;

        public string PhoneNumber { get; set; }
        public string PhoneCodeHash { get; set; }
        public string PhoneCode { get; set; }
        public TlAuthorization Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumber = StringUtil.Deserialize(br);
            PhoneCodeHash = StringUtil.Deserialize(br);
            PhoneCode = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(PhoneNumber, bw);
            StringUtil.Serialize(PhoneCodeHash, bw);
            StringUtil.Serialize(PhoneCode, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAuthorization) ObjectUtils.DeserializeObject(br);
        }
    }
}