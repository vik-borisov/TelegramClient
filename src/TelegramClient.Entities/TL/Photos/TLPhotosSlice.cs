using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(352657236)]
    public class TlPhotosSlice : TlAbsPhotos
    {
        public override int Constructor => 352657236;

        public int Count { get; set; }
        public TlVector<TlAbsPhoto> Photos { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Photos = ObjectUtils.DeserializeVector<TlAbsPhoto>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Photos, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}