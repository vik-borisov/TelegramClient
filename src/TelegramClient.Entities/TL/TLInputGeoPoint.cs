using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-206066487)]
    public class TlInputGeoPoint : TlAbsInputGeoPoint
    {
        public override int Constructor => -206066487;

        public double Lat { get; set; }
        public double Long { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Lat = br.ReadDouble();
            Long = br.ReadDouble();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Lat);
            bw.Write(Long);
        }
    }
}