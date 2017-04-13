using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(541710092)]
    public class TlGeoPoint : TlAbsGeoPoint
    {
        public override int Constructor => 541710092;

        public double Long { get; set; }
        public double Lat { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Long = br.ReadDouble();
            Lat = br.ReadDouble();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Long);
            bw.Write(Lat);
        }
    }
}