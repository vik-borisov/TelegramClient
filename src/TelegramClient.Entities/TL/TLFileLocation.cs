using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1406570614)]
    public class TlFileLocation : TlAbsFileLocation
    {
        public override int Constructor => 1406570614;

        public int DcId { get; set; }
        public long VolumeId { get; set; }
        public int LocalId { get; set; }
        public long Secret { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            DcId = br.ReadInt32();
            VolumeId = br.ReadInt64();
            LocalId = br.ReadInt32();
            Secret = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(DcId);
            bw.Write(VolumeId);
            bw.Write(LocalId);
            bw.Write(Secret);
        }
    }
}