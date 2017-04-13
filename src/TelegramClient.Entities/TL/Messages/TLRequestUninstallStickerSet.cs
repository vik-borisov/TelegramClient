using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-110209570)]
    public class TlRequestUninstallStickerSet : TlMethod
    {
        public override int Constructor => -110209570;

        public TlAbsInputStickerSet Stickerset { get; set; }
        public bool Response { get; set; }


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
            Response = BoolUtil.Deserialize(br);
        }
    }
}