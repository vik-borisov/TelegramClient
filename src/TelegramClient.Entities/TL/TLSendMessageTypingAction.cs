using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(381645902)]
    public class TlSendMessageTypingAction : TlAbsSendMessageAction
    {
        public override int Constructor => 381645902;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}