using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1038967584)]
    public class TlMessageMediaEmpty : TlAbsMessageMedia
    {
        public override int Constructor => 1038967584;


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