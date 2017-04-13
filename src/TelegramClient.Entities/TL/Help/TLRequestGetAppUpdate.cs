using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-1372724842)]
    public class TlRequestGetAppUpdate : TlMethod
    {
        public override int Constructor => -1372724842;

        public TlAbsAppUpdate Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsAppUpdate) ObjectUtils.DeserializeObject(br);
        }
    }
}