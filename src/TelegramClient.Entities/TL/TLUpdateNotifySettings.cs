using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1094555409)]
    public class TlUpdateNotifySettings : TlAbsUpdate
    {
        public override int Constructor => -1094555409;

        public TlAbsNotifyPeer Peer { get; set; }
        public TlAbsPeerNotifySettings NotifySettings { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsNotifyPeer) ObjectUtils.DeserializeObject(br);
            NotifySettings = (TlAbsPeerNotifySettings) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(NotifySettings, bw);
        }
    }
}