using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(2031374829)]
    public class TlRequestSetEncryptedTyping : TlMethod
    {
        public override int Constructor => 2031374829;

        public TlInputEncryptedChat Peer { get; set; }
        public bool Typing { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlInputEncryptedChat) ObjectUtils.DeserializeObject(br);
            Typing = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            BoolUtil.Serialize(Typing, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}