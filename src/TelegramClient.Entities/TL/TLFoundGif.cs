using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(372165663)]
    public class TlFoundGif : TlAbsFoundGif
    {
        public override int Constructor => 372165663;

        public string Url { get; set; }
        public string ThumbUrl { get; set; }
        public string ContentUrl { get; set; }
        public string ContentType { get; set; }
        public int W { get; set; }
        public int H { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Url = StringUtil.Deserialize(br);
            ThumbUrl = StringUtil.Deserialize(br);
            ContentUrl = StringUtil.Deserialize(br);
            ContentType = StringUtil.Deserialize(br);
            W = br.ReadInt32();
            H = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Url, bw);
            StringUtil.Serialize(ThumbUrl, bw);
            StringUtil.Serialize(ContentUrl, bw);
            StringUtil.Serialize(ContentType, bw);
            bw.Write(W);
            bw.Write(H);
        }
    }
}