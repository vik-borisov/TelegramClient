using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(946083368)]
    public class TlStickerSetInstallResultSuccess : TlAbsStickerSetInstallResult
    {
        public override int Constructor => 946083368;


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