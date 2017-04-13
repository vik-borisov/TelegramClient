using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1889961234)]
    public class TlPeerNotifySettingsEmpty : TlAbsPeerNotifySettings
    {
        public override int Constructor => 1889961234;


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