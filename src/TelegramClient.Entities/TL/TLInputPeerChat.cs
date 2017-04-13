using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(396093539)]
    public class TlInputPeerChat : TlAbsInputPeer
    {
        public override int Constructor => 396093539;

        public int ChatId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
        }
    }
}