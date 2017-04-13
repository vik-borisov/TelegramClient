using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(935395612)]
    public class TlChatPhotoEmpty : TlAbsChatPhoto
    {
        public override int Constructor => 935395612;


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