using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1837345356)]
    public class TlInputChatUploadedPhoto : TlAbsInputChatPhoto
    {
        public override int Constructor => -1837345356;

        public TlAbsInputFile File { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            File = (TlAbsInputFile) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(File, bw);
        }
    }
}