using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1160714821)]
    public class TlPeerChat : TlAbsPeer
    {
        public override int Constructor => -1160714821;

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