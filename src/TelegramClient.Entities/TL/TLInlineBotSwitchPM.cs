using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1008755359)]
    public class TlInlineBotSwitchPm : TlObject
    {
        public override int Constructor => 1008755359;

        public string Text { get; set; }
        public string StartParam { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Text = StringUtil.Deserialize(br);
            StartParam = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Text, bw);
            StringUtil.Serialize(StartParam, bw);
        }
    }
}