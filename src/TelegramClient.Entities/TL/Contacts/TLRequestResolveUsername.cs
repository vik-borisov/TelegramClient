using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-113456221)]
    public class TlRequestResolveUsername : TlMethod
    {
        public override int Constructor => -113456221;

        public string Username { get; set; }
        public TlResolvedPeer Response { get; set; }


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
            Response = (TlResolvedPeer) ObjectUtils.DeserializeObject(br);
        }
    }
}