using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-3644025)]
    public class TlInputMessagesFilterGif : TlAbsMessagesFilter
    {
        public override int Constructor => -3644025;


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