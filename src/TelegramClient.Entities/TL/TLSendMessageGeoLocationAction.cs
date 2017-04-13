using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(393186209)]
    public class TlSendMessageGeoLocationAction : TlAbsSendMessageAction
    {
        public override int Constructor => 393186209;


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