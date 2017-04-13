using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-715532088)]
    public class TlUserProfilePhoto : TlAbsUserProfilePhoto
    {
        public override int Constructor => -715532088;

        public long PhotoId { get; set; }
        public TlAbsFileLocation PhotoSmall { get; set; }
        public TlAbsFileLocation PhotoBig { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhotoId = br.ReadInt64();
            PhotoSmall = (TlAbsFileLocation) ObjectUtils.DeserializeObject(br);
            PhotoBig = (TlAbsFileLocation) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(PhotoId);
            ObjectUtils.SerializeObject(PhotoSmall, bw);
            ObjectUtils.SerializeObject(PhotoBig, bw);
        }
    }
}