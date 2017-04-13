using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-2131957734)]
    public class TlUpdateUserBlocked : TlAbsUpdate
    {
        public override int Constructor => -2131957734;

        public int UserId { get; set; }
        public bool Blocked { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Blocked = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            BoolUtil.Serialize(Blocked, bw);
        }
    }
}