using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-236044656)]
    public class TlTermsOfService : TlObject
    {
        public override int Constructor => -236044656;

        public string Text { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Text = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Text, bw);
        }
    }
}