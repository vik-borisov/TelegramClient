using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(1040964988)]
    public class TlRequestUpdateUsername : TlMethod
    {
        public override int Constructor => 1040964988;

        public string Username { get; set; }
        public TlAbsUser Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Username = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Username, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUser) ObjectUtils.DeserializeObject(br);
        }
    }
}