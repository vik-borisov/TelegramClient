using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(904138920)]
    public class TlStickerSetInstallResultArchive : TlAbsStickerSetInstallResult
    {
        public override int Constructor => 904138920;

        public TlVector<TlAbsStickerSetCovered> Sets { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Sets = ObjectUtils.DeserializeVector<TlAbsStickerSetCovered>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Sets, bw);
        }
    }
}