using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-104578748)]
    public class TlInputMediaGeoPoint : TlAbsInputMedia
    {
        public override int Constructor => -104578748;

        public TlAbsInputGeoPoint GeoPoint { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            GeoPoint = (TlAbsInputGeoPoint) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(GeoPoint, bw);
        }
    }
}