using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(-1916114267)]
    public class TlPhotos : TlAbsPhotos
    {
        public override int Constructor => -1916114267;

        public TlVector<TlAbsPhoto> Photos { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Photos = ObjectUtils.DeserializeVector<TlAbsPhoto>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Photos, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}