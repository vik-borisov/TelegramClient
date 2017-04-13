using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-748155807)]
    public class TlContactStatus : TlObject
    {
        public override int Constructor => -748155807;

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