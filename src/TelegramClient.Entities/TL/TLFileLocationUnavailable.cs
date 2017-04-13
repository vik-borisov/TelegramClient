using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2086234950)]
    public class TlFileLocationUnavailable : TlAbsFileLocation
    {
        public override int Constructor => 2086234950;

        public long VolumeId { get; set; }
        public int LocalId { get; set; }
        public long Secret { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            VolumeId = br.ReadInt64();
            LocalId = br.ReadInt32();
            Secret = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(VolumeId);
            bw.Write(LocalId);
            bw.Write(Secret);
        }
    }
}