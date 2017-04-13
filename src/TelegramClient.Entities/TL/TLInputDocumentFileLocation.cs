using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1125058340)]
    public class TlInputDocumentFileLocation : TlAbsInputFileLocation
    {
        public override int Constructor => 1125058340;

        public long Id { get; set; }
        public long AccessHash { get; set; }
        public int Version { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
            Version = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(AccessHash);
            bw.Write(Version);
        }
    }
}