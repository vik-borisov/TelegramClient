using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-866424884)]
    public class TlRequestGetAttachedStickers : TlMethod
    {
        public override int Constructor => -866424884;

        public TlAbsInputStickeredMedia Media { get; set; }
        public TlVector<TlAbsStickerSetCovered> Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Media = (TlAbsInputStickeredMedia) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Media, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = ObjectUtils.DeserializeVector<TlAbsStickerSetCovered>(br);
        }
    }
}