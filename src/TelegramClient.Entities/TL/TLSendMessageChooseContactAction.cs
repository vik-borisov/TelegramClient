using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1653390447)]
    public class TlSendMessageChooseContactAction : TlAbsSendMessageAction
    {
        public override int Constructor => 1653390447;


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