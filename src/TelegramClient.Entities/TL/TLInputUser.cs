using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-668391402)]
    public class TlInputUser : TlAbsInputUser
    {
        public override int Constructor => -668391402;

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