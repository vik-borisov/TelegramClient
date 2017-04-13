using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1378534221)]
    public class TlPeerNotifyEventsEmpty : TlAbsPeerNotifyEvents
    {
        public override int Constructor => -1378534221;


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