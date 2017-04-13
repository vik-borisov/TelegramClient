using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1551737264)]
    public class TlRequestSetTyping : TlMethod
    {
        public override int Constructor => -1551737264;

        public TlAbsInputPeer Peer { get; set; }
        public TlAbsSendMessageAction Action { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Action = (TlAbsSendMessageAction) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(Action, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}