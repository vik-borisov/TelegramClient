using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(410618194)]
    public class TlInputDocument : TlAbsInputDocument
    {
        public override int Constructor => 410618194;

        public long Id { get; set; }
        public long AccessHash { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(AccessHash);
        }
    }
}