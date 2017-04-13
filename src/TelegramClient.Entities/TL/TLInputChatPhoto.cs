using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1991004873)]
    public class TlInputChatPhoto : TlAbsInputChatPhoto
    {
        public override int Constructor => -1991004873;

        public TlAbsInputPhoto Id { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputPhoto) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }
    }
}