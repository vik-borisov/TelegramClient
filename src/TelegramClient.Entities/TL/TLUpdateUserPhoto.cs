using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1791935732)]
    public class TlUpdateUserPhoto : TlAbsUpdate
    {
        public override int Constructor => -1791935732;

        public int UserId { get; set; }
        public int Date { get; set; }
        public TlAbsUserProfilePhoto Photo { get; set; }
        public bool Previous { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Date = br.ReadInt32();
            Photo = (TlAbsUserProfilePhoto) ObjectUtils.DeserializeObject(br);
            Previous = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            bw.Write(Date);
            ObjectUtils.SerializeObject(Photo, bw);
            BoolUtil.Serialize(Previous, bw);
        }
    }
}