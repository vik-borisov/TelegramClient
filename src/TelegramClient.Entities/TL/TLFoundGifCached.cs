using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1670052855)]
    public class TlFoundGifCached : TlAbsFoundGif
    {
        public override int Constructor => -1670052855;

        public string Url { get; set; }
        public TlAbsPhoto Photo { get; set; }
        public TlAbsDocument Document { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Url = StringUtil.Deserialize(br);
            Photo = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
            Document = (TlAbsDocument) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Url, bw);
            ObjectUtils.SerializeObject(Photo, bw);
            ObjectUtils.SerializeObject(Document, bw);
        }
    }
}