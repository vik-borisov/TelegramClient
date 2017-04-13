using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(313694676)]
    public class TlStickerPack : TlObject
    {
        public override int Constructor => 313694676;

        public string Emoticon { get; set; }
        public TlVector<long> Documents { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Emoticon = StringUtil.Deserialize(br);
            Documents = ObjectUtils.DeserializeVector<long>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Emoticon, bw);
            ObjectUtils.SerializeObject(Documents, bw);
        }
    }
}