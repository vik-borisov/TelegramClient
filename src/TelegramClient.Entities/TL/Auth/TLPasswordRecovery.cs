using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(326715557)]
    public class TlPasswordRecovery : TlObject
    {
        public override int Constructor => 326715557;

        public string EmailPattern { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            EmailPattern = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(EmailPattern, bw);
        }
    }
}