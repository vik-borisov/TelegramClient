using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-106911223)]
    public class TlRequestAddChatUser : TlMethod
    {
        public override int Constructor => -106911223;

        public int ChatId { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public int FwdLimit { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            FwdLimit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            ObjectUtils.SerializeObject(UserId, bw);
            bw.Write(FwdLimit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}