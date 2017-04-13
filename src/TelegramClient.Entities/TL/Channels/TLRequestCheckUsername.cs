using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(283557164)]
    public class TlRequestCheckUsername : TlMethod
    {
        public override int Constructor => 283557164;

        public TlAbsInputChannel Channel { get; set; }
        public string Username { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Username = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            StringUtil.Serialize(Username, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}