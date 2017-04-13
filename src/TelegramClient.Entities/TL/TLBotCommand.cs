using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1032140601)]
    public class TlBotCommand : TlObject
    {
        public override int Constructor => -1032140601;

        public string Command { get; set; }
        public string Description { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Command = StringUtil.Deserialize(br);
            Description = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Command, bw);
            StringUtil.Serialize(Description, bw);
        }
    }
}