using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1970352846)]
    public class TlStickers : TlAbsStickers
    {
        public override int Constructor => -1970352846;

        public string Hash { get; set; }
        public TlVector<TlAbsDocument> Stickers { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = StringUtil.Deserialize(br);
            Stickers = ObjectUtils.DeserializeVector<TlAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Hash, bw);
            ObjectUtils.SerializeObject(Stickers, bw);
        }
    }
}