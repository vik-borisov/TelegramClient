using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1336154098)]
    public class TlInputBotInlineResultGame : TlAbsInputBotInlineResult
    {
        public override int Constructor => 1336154098;

        public string Id { get; set; }
        public string ShortName { get; set; }
        public TlAbsInputBotInlineMessage SendMessage { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = StringUtil.Deserialize(br);
            ShortName = StringUtil.Deserialize(br);
            SendMessage = (TlAbsInputBotInlineMessage) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Id, bw);
            StringUtil.Serialize(ShortName, bw);
            ObjectUtils.SerializeObject(SendMessage, bw);
        }
    }
}