using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(313765169)]
    public class TlRequestGetNotifySettings : TlMethod
    {
        public override int Constructor => 313765169;

        public TlAbsInputNotifyPeer Peer { get; set; }
        public TlAbsPeerNotifySettings Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputNotifyPeer) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsPeerNotifySettings) ObjectUtils.DeserializeObject(br);
        }
    }
}