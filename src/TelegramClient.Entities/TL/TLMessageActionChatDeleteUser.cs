using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1297179892)]
    public class TlMessageActionChatDeleteUser : TlAbsMessageAction
    {
        public override int Constructor => -1297179892;

        public int UserId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
        }
    }
}