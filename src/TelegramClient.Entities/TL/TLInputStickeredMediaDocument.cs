using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(70813275)]
    public class TlInputStickeredMediaDocument : TlAbsInputStickeredMedia
    {
        public override int Constructor => 70813275;

        public TlAbsInputDocument Id { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputDocument) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }
    }
}