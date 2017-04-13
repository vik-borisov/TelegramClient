using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1781355374)]
    public class TlMessageActionChannelCreate : TlAbsMessageAction
    {
        public override int Constructor => -1781355374;

        public string Title { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Title = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Title, bw);
        }
    }
}