using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1649296275)]
    public class TlPeerUser : TlAbsPeer
    {
        public override int Constructor => -1649296275;

        public int UserId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
        }
    }
}