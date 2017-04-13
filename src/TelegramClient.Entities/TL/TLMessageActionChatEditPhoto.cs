using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2144015272)]
    public class TlMessageActionChatEditPhoto : TlAbsMessageAction
    {
        public override int Constructor => 2144015272;

        public TlAbsPhoto Photo { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Photo = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Photo, bw);
        }
    }
}