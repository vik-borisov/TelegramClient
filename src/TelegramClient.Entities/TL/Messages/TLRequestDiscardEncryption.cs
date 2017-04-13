using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-304536635)]
    public class TlRequestDiscardEncryption : TlMethod
    {
        public override int Constructor => -304536635;

        public int ChatId { get; set; }
        public bool Response { get; set; }


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
            Response = BoolUtil.Deserialize(br);
        }
    }
}