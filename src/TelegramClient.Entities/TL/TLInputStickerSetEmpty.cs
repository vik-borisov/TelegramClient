using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-4838507)]
    public class TlInputStickerSetEmpty : TlAbsInputStickerSet
    {
        public override int Constructor => -4838507;


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