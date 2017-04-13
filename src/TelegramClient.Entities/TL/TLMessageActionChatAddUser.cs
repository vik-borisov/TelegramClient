using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1217033015)]
    public class TlMessageActionChatAddUser : TlAbsMessageAction
    {
        public override int Constructor => 1217033015;

        public TlVector<int> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Users = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}