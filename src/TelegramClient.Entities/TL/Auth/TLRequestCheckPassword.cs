using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(174260510)]
    public class TlRequestCheckPassword : TlMethod
    {
        public override int Constructor => 174260510;

        public byte[] PasswordHash { get; set; }
        public TlAuthorization Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PasswordHash = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BytesUtil.Serialize(PasswordHash, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAuthorization) ObjectUtils.DeserializeObject(br);
        }
    }
}