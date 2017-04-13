using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1662637586)]
    public class TlDocumentAttributeSticker : TlAbsDocumentAttribute
    {
        public override int Constructor => 1662637586;

        public int Flags { get; set; }
        public bool Mask { get; set; }
        public string Alt { get; set; }
        public TlAbsInputStickerSet Stickerset { get; set; }
        public TlMaskCoords MaskCoords { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Mask ? Flags | 2 : Flags & ~2;
            Flags = MaskCoords != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Mask = (Flags & 2) != 0;
            Alt = StringUtil.Deserialize(br);
            Stickerset = (TlAbsInputStickerSet) ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                MaskCoords = (TlMaskCoords) ObjectUtils.DeserializeObject(br);
            else
                MaskCoords = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            StringUtil.Serialize(Alt, bw);
            ObjectUtils.SerializeObject(Stickerset, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(MaskCoords, bw);
        }
    }
}