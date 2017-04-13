using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-1878523231)]
    public class TlBlockedSlice : TlAbsBlocked
    {
        public override int Constructor => -1878523231;

        public int Count { get; set; }
        public TlVector<TlContactBlocked> Blocked { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Blocked = ObjectUtils.DeserializeVector<TlContactBlocked>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Blocked, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}