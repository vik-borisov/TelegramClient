using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(82699215)]
    public class TlFeaturedStickersNotModified : TlAbsFeaturedStickers
    {
        public override int Constructor => 82699215;


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