using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-244016606)]
    public class TlStickersNotModified : TlAbsStickers
    {
        public override int Constructor => -244016606;


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