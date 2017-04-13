using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1462213465)]
    public class TlInputBotInlineResultPhoto : TlAbsInputBotInlineResult
    {
        public override int Constructor => -1462213465;

        public string Id { get; set; }
        public string Type { get; set; }
        public TlAbsInputPhoto Photo { get; set; }
        public TlAbsInputBotInlineMessage SendMessage { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = StringUtil.Deserialize(br);
            Type = StringUtil.Deserialize(br);
            Photo = (TlAbsInputPhoto) ObjectUtils.DeserializeObject(br);
            SendMessage = (TlAbsInputBotInlineMessage) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Id, bw);
            StringUtil.Serialize(Type, bw);
            ObjectUtils.SerializeObject(Photo, bw);
            ObjectUtils.SerializeObject(SendMessage, bw);
        }
    }
}