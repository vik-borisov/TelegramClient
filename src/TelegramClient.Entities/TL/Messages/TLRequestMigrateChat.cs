using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(363051235)]
    public class TlRequestMigrateChat : TlMethod
    {
        public override int Constructor => 363051235;

        public int ChatId { get; set; }
        public TlAbsUpdates Response { get; set; }


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
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}