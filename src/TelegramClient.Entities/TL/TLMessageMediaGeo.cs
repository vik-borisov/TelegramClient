using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1457575028)]
    public class TlMessageMediaGeo : TlAbsMessageMedia
    {
        public override int Constructor => 1457575028;

        public TlAbsGeoPoint Geo { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Geo = (TlAbsGeoPoint) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Geo, bw);
        }
    }
}