using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-1764049896)]
    public class TlNoPassword : TlAbsPassword
    {
        public override int Constructor => -1764049896;

        public byte[] NewSalt { get; set; }
        public string EmailUnconfirmedPattern { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            NewSalt = BytesUtil.Deserialize(br);
            EmailUnconfirmedPattern = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BytesUtil.Serialize(NewSalt, bw);
            StringUtil.Serialize(EmailUnconfirmedPattern, bw);
        }
    }
}