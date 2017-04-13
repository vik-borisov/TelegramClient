using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1780220945)]
    public class TlMessageActionChatDeletePhoto : TlAbsMessageAction
    {
        public override int Constructor => -1780220945;


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