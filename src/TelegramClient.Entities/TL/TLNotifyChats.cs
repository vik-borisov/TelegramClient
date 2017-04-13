using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1073230141)]
    public class TlNotifyChats : TlAbsNotifyPeer
    {
        public override int Constructor => -1073230141;


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