using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-395694988)]
    public class TlInputPeerNotifyEventsAll : TlAbsInputPeerNotifyEvents
    {
        public override int Constructor => -395694988;


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