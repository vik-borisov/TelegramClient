using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-2044933984)]
    public class TlInputStickerSetShortName : TlAbsInputStickerSet
    {
        public override int Constructor => -2044933984;

        public string ShortName { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ShortName = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(ShortName, bw);
        }
    }
}