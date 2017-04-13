using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1020139510)]
    public class TlInputGameShortName : TlAbsInputGame
    {
        public override int Constructor => -1020139510;

        public TlAbsInputUser BotId { get; set; }
        public string ShortName { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            BotId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            ShortName = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(BotId, bw);
            StringUtil.Serialize(ShortName, bw);
        }
    }
}