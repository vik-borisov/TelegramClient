using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(998448230)]
    public class TlRequestGetFullChat : TlMethod
    {
        public override int Constructor => 998448230;

        public int ChatId { get; set; }
        public TlChatFull Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlChatFull) ObjectUtils.DeserializeObject(br);
        }
    }
}