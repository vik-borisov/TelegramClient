using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2072935910)]
    public class TlInputPeerUser : TlAbsInputPeer
    {
        public override int Constructor => 2072935910;

        public int UserId { get; set; }
        public long AccessHash { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            AccessHash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            bw.Write(AccessHash);
        }
    }
}