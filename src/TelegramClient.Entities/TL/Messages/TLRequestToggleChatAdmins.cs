using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-326379039)]
    public class TlRequestToggleChatAdmins : TlMethod
    {
        public override int Constructor => -326379039;

        public int ChatId { get; set; }
        public bool Enabled { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            Enabled = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            BoolUtil.Serialize(Enabled, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}