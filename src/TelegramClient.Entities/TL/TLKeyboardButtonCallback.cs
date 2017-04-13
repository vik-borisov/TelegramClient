using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1748655686)]
    public class TlKeyboardButtonCallback : TlAbsKeyboardButton
    {
        public override int Constructor => 1748655686;

        public string Text { get; set; }
        public byte[] Data { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Text = StringUtil.Deserialize(br);
            Data = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Text, bw);
            BytesUtil.Serialize(Data, bw);
        }
    }
}