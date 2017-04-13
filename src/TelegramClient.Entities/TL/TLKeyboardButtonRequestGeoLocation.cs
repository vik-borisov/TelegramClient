using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-59151553)]
    public class TlKeyboardButtonRequestGeoLocation : TlAbsKeyboardButton
    {
        public override int Constructor => -59151553;

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