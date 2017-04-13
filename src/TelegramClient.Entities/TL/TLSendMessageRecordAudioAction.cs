using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-718310409)]
    public class TlSendMessageRecordAudioAction : TlAbsSendMessageAction
    {
        public override int Constructor => -718310409;


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