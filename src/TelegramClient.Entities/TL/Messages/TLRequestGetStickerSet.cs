using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(639215886)]
    public class TlRequestGetStickerSet : TlMethod
    {
        public override int Constructor => 639215886;

        public TlAbsInputStickerSet Stickerset { get; set; }
        public TlStickerSet Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Stickerset = (TlAbsInputStickerSet) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Stickerset, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlStickerSet) ObjectUtils.DeserializeObject(br);
        }
    }
}