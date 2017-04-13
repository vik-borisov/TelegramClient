using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(539045032)]
    public class TlPhoto : TlObject
    {
        public override int Constructor => 539045032;

        public TlAbsPhoto Photo { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Photo = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Photo, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}