using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1694474197)]
    public class TlChats : TlObject
    {
        public override int Constructor => 1694474197;

        public TlVector<TlAbsChat> Chats { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Chats = ObjectUtils.DeserializeVector<TlAbsChat>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Chats, bw);
        }
    }
}