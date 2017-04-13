using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-946871200)]
    public class TlRequestInstallStickerSet : TlMethod
    {
        public override int Constructor => -946871200;

        public TlAbsInputStickerSet Stickerset { get; set; }
        public bool Archived { get; set; }
        public TlAbsStickerSetInstallResult Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Stickerset = (TlAbsInputStickerSet) ObjectUtils.DeserializeObject(br);
            Archived = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Stickerset, bw);
            BoolUtil.Serialize(Archived, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsStickerSetInstallResult) ObjectUtils.DeserializeObject(br);
        }
    }
}