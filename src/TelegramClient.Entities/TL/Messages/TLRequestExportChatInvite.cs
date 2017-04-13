using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(2106086025)]
    public class TlRequestExportChatInvite : TlMethod
    {
        public override int Constructor => 2106086025;

        public int ChatId { get; set; }
        public TlAbsExportedChatInvite Response { get; set; }


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
            Response = (TlAbsExportedChatInvite) ObjectUtils.DeserializeObject(br);
        }
    }
}