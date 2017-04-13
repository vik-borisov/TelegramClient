using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1877286395)]
    public class TlRequestCheckPhone : TlMethod
    {
        public override int Constructor => 1877286395;

        public string PhoneNumber { get; set; }
        public TlCheckedPhone Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumber = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(PhoneNumber, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlCheckedPhone) ObjectUtils.DeserializeObject(br);
        }
    }
}