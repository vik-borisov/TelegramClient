using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-648121413)]
    public class TlInputMessagesFilterPhotoVideoDocuments : TlAbsMessagesFilter
    {
        public override int Constructor => -648121413;


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