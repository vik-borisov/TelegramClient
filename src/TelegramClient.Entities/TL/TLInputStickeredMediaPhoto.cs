using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1251549527)]
    public class TlInputStickeredMediaPhoto : TlAbsInputStickeredMedia
    {
        public override int Constructor => 1251549527;

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