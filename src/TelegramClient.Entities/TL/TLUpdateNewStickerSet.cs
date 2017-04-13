using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1753886890)]
    public class TlUpdateNewStickerSet : TlAbsUpdate
    {
        public override int Constructor => 1753886890;

        public Messages.TlStickerSet Stickerset { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Stickerset = (Messages.TlStickerSet) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Stickerset, bw);
        }
    }
}