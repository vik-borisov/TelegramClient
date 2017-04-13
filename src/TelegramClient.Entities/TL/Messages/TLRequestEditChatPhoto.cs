using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-900957736)]
    public class TlRequestEditChatPhoto : TlMethod
    {
        public override int Constructor => -900957736;

        public int ChatId { get; set; }
        public TlAbsInputChatPhoto Photo { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            Photo = (TlAbsInputChatPhoto) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            ObjectUtils.SerializeObject(Photo, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}