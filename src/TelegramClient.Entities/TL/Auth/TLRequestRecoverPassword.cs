using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1319464594)]
    public class TlRequestRecoverPassword : TlMethod
    {
        public override int Constructor => 1319464594;

        public string Code { get; set; }
        public TlAuthorization Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Code = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Code, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAuthorization) ObjectUtils.DeserializeObject(br);
        }
    }
}