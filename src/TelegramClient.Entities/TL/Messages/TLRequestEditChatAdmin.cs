using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1444503762)]
    public class TlRequestEditChatAdmin : TlMethod
    {
        public override int Constructor => -1444503762;

        public int ChatId { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public bool IsAdmin { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            IsAdmin = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            ObjectUtils.SerializeObject(UserId, bw);
            BoolUtil.Serialize(IsAdmin, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}