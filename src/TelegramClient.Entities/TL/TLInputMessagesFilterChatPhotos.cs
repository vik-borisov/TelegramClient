using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(975236280)]
    public class TlInputMessagesFilterChatPhotos : TlAbsMessagesFilter
    {
        public override int Constructor => 975236280;


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