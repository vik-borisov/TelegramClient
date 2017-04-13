using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(1181279933)]
    public class TlAppChangelog : TlAbsAppChangelog
    {
        public override int Constructor => 1181279933;

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