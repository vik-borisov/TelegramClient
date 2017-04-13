using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1051570619)]
    public class TlRequestCheckChatInvite : TlMethod
    {
        public override int Constructor => 1051570619;

        public string Hash { get; set; }
        public TlAbsChatInvite Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Hash, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsChatInvite) ObjectUtils.DeserializeObject(br);
        }
    }
}