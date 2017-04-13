using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(-2016444625)]
    public class TlRequestDeletePhotos : TlMethod
    {
        public override int Constructor => -2016444625;

        public TlVector<TlAbsInputPhoto> Id { get; set; }
        public TlVector<long> Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = ObjectUtils.DeserializeVector<TlAbsInputPhoto>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = ObjectUtils.DeserializeVector<long>(br);
        }
    }
}