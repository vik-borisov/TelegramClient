using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(655677548)]
    public class TlRequestCheckUsername : TlMethod
    {
        public override int Constructor => 655677548;

        public string Username { get; set; }
        public bool Response { get; set; }


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
            Response = BoolUtil.Deserialize(br);
        }
    }
}