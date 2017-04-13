using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1928391342)]
    public class TlInputDocumentEmpty : TlAbsInputDocument
    {
        public override int Constructor => 1928391342;


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