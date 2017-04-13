using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-530505962)]
    public class TlRequestDeleteChatUser : TlMethod
    {
        public override int Constructor => -530505962;

        public int ChatId { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            ObjectUtils.SerializeObject(UserId, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}