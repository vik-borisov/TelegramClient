using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(480546647)]
    public class TlInputChatPhotoEmpty : TlAbsInputChatPhoto
    {
        public override int Constructor => 480546647;


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