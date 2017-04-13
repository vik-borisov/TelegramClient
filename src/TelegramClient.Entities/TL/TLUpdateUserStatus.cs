using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(469489699)]
    public class TlUpdateUserStatus : TlAbsUpdate
    {
        public override int Constructor => 469489699;

        public int UserId { get; set; }
        public TlAbsUserStatus Status { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Status = (TlAbsUserStatus) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            ObjectUtils.SerializeObject(Status, bw);
        }
    }
}