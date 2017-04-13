using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1632839530)]
    public class TlChatPhoto : TlAbsChatPhoto
    {
        public override int Constructor => 1632839530;

        public TlAbsFileLocation PhotoSmall { get; set; }
        public TlAbsFileLocation PhotoBig { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhotoSmall = (TlAbsFileLocation) ObjectUtils.DeserializeObject(br);
            PhotoBig = (TlAbsFileLocation) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(PhotoSmall, bw);
            ObjectUtils.SerializeObject(PhotoBig, bw);
        }
    }
}