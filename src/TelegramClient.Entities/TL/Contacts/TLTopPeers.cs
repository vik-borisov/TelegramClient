using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(1891070632)]
    public class TlTopPeers : TlAbsTopPeers
    {
        public override int Constructor => 1891070632;

        public TlVector<TlTopPeerCategoryPeers> Categories { get; set; }
        public TlVector<TlAbsChat> Chats { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Categories = ObjectUtils.DeserializeVector<TlTopPeerCategoryPeers>(br);
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Categories, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}