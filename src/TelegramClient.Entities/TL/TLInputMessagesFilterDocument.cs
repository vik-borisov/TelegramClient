using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1629621880)]
    public class TlInputMessagesFilterDocument : TlAbsMessagesFilter
    {
        public override int Constructor => -1629621880;


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