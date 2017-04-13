using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(590459437)]
    public class TlPhotoEmpty : TlAbsPhoto
    {
        public override int Constructor => 590459437;

        public long Id { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
        }
    }
}