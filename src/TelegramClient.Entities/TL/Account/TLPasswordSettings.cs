using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-1212732749)]
    public class TlPasswordSettings : TlObject
    {
        public override int Constructor => -1212732749;

        public string Email { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Email = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Email, bw);
        }
    }
}