using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-457104426)]
    public class TlInputGeoPointEmpty : TlAbsInputGeoPoint
    {
        public override int Constructor => -457104426;


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