using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-44119819)]
    public class TlSendMessageCancelAction : TlAbsSendMessageAction
    {
        public override int Constructor => -44119819;


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