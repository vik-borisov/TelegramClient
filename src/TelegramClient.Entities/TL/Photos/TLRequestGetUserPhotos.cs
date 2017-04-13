using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(-1848823128)]
    public class TlRequestGetUserPhotos : TlMethod
    {
        public override int Constructor => -1848823128;

        public TlAbsInputUser UserId { get; set; }
        public int Offset { get; set; }
        public long MaxId { get; set; }
        public int Limit { get; set; }
        public TlAbsPhotos Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Offset = br.ReadInt32();
            MaxId = br.ReadInt64();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(UserId, bw);
            bw.Write(Offset);
            bw.Write(MaxId);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsPhotos) ObjectUtils.DeserializeObject(br);
        }
    }
}