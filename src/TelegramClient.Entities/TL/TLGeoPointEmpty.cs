using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(286776671)]
    public class TlGeoPointEmpty : TlAbsGeoPoint
    {
        public override int Constructor => 286776671;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}