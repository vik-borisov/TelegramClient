using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1830677896)]
    public class TlPeerNotifyEventsAll : TlAbsPeerNotifyEvents
    {
        public override int Constructor => 1830677896;


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