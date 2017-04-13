using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(1328726168)]
    public class TlRequestUploadProfilePhoto : TlMethod
    {
        public override int Constructor => 1328726168;

        public TlAbsInputFile File { get; set; }
        public TlPhoto Response { get; set; }


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

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlPhoto) ObjectUtils.DeserializeObject(br);
        }
    }
}