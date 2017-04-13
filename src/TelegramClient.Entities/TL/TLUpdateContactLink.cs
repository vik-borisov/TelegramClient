using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1657903163)]
    public class TlUpdateContactLink : TlAbsUpdate
    {
        public override int Constructor => -1657903163;

        public int UserId { get; set; }
        public TlAbsContactLink MyLink { get; set; }
        public TlAbsContactLink ForeignLink { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            MyLink = (TlAbsContactLink) ObjectUtils.DeserializeObject(br);
            ForeignLink = (TlAbsContactLink) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            ObjectUtils.SerializeObject(MyLink, bw);
            ObjectUtils.SerializeObject(ForeignLink, bw);
        }
    }
}