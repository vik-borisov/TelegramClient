using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(766298703)]
    public class TlRequestGetFeaturedStickers : TlMethod
    {
        public override int Constructor => 766298703;

        public int Hash { get; set; }
        public TlAbsFeaturedStickers Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsFeaturedStickers) ObjectUtils.DeserializeObject(br);
        }
    }
}