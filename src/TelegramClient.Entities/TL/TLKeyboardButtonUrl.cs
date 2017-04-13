using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(629866245)]
    public class TlKeyboardButtonUrl : TlAbsKeyboardButton
    {
        public override int Constructor => 629866245;

        public string Text { get; set; }
        public string Url { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Text = StringUtil.Deserialize(br);
            Url = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Text, bw);
            StringUtil.Serialize(Url, bw);
        }
    }
}