using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-599447467)]
    public class TlRequestEditChatTitle : TlMethod
    {
        public override int Constructor => -599447467;

        public int ChatId { get; set; }
        public string Title { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            Title = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            StringUtil.Serialize(Title, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}