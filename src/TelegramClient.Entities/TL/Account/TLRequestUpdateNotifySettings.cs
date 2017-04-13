using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-2067899501)]
    public class TlRequestUpdateNotifySettings : TlMethod
    {
        public override int Constructor => -2067899501;

        public TlAbsInputNotifyPeer Peer { get; set; }
        public TlInputPeerNotifySettings Settings { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputNotifyPeer) ObjectUtils.DeserializeObject(br);
            Settings = (TlInputPeerNotifySettings) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(Settings, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}