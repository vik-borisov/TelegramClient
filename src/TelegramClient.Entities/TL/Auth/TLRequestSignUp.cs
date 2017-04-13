using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(453408308)]
    public class TlRequestSignUp : TlMethod
    {
        public override int Constructor => 453408308;

        public string PhoneNumber { get; set; }
        public string PhoneCodeHash { get; set; }
        public string PhoneCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TlAuthorization Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumber = StringUtil.Deserialize(br);
            PhoneCodeHash = StringUtil.Deserialize(br);
            PhoneCode = StringUtil.Deserialize(br);
            FirstName = StringUtil.Deserialize(br);
            LastName = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(PhoneNumber, bw);
            StringUtil.Serialize(PhoneCodeHash, bw);
            StringUtil.Serialize(PhoneCode, bw);
            StringUtil.Serialize(FirstName, bw);
            StringUtil.Serialize(LastName, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAuthorization) ObjectUtils.DeserializeObject(br);
        }
    }
}