using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1548249383)]
    public class TlUpdateUserTyping : TlAbsUpdate
    {
        public override int Constructor => 1548249383;

        public int UserId { get; set; }
        public TlAbsSendMessageAction Action { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Action = (TlAbsSendMessageAction) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            ObjectUtils.SerializeObject(Action, bw);
        }
    }
}