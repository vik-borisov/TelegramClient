using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(479598769)]
    public class TlRequestGetAllStickers : TlMethod
    {
        public override int Constructor => 479598769;

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