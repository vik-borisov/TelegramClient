using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1358175439)]
    public class TlKeyboardButtonGame : TlAbsKeyboardButton
    {
        public override int Constructor => 1358175439;

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