using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-395967805)]
    public class TlAllStickersNotModified : TlAbsAllStickers
    {
        public override int Constructor => -395967805;


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