using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1056025023)]
    public class TlRequestResendCode : TlMethod
    {
        public override int Constructor => 1056025023;

        public string PhoneNumber { get; set; }
        public string PhoneCodeHash { get; set; }
        public TlSentCode Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumber = StringUtil.Deserialize(br);
            PhoneCodeHash = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(PhoneNumber, bw);
            StringUtil.Serialize(PhoneCodeHash, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlSentCode) ObjectUtils.DeserializeObject(br);
        }
    }
}