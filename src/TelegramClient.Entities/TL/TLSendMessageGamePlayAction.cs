using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-580219064)]
    public class TlSendMessageGamePlayAction : TlAbsSendMessageAction
    {
        public override int Constructor => -580219064;


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