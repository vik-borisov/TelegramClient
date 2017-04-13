using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-265263912)]
    public class TlInputPeerNotifyEventsEmpty : TlAbsInputPeerNotifyEvents
    {
        public override int Constructor => -265263912;


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