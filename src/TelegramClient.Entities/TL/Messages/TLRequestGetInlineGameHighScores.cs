using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(258170395)]
    public class TlRequestGetInlineGameHighScores : TlMethod
    {
        public override int Constructor => 258170395;

        public TlInputBotInlineMessageId Id { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlHighScores Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlInputBotInlineMessageId) ObjectUtils.DeserializeObject(br);
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
            ObjectUtils.SerializeObject(UserId, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlHighScores) ObjectUtils.DeserializeObject(br);
        }
    }
}