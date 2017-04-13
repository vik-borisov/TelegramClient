using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(922273905)]
    public class TlDocumentEmpty : TlAbsDocument
    {
        public override int Constructor => 922273905;

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