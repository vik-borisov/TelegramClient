using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1706608543)]
    public class TlRequestGetMaskStickers : TlMethod
    {
        public override int Constructor => 1706608543;

        public int Hash { get; set; }
        public TlAbsAllStickers Response { get; set; }


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
            Response = (TlAbsAllStickers) ObjectUtils.DeserializeObject(br);
        }
    }
}