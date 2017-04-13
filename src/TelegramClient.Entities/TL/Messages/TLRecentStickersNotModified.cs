using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(186120336)]
    public class TlRecentStickersNotModified : TlAbsRecentStickers
    {
        public override int Constructor => 186120336;


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