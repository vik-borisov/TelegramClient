using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1957577280)]
    public class TlUpdates : TlAbsUpdates
    {
        public override int Constructor => 1957577280;

        public TlVector<TlAbsUpdate> Updates { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public int Date { get; set; }
        public int Seq { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Updates = ObjectUtils.DeserializeVector<TlAbsUpdate>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Date = br.ReadInt32();
            Seq = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Updates, bw);
            ObjectUtils.SerializeObject(Users, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            bw.Write(Date);
            bw.Write(Seq);
        }
    }
}