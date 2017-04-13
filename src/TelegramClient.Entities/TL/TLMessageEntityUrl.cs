using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1859134776)]
    public class TlMessageEntityUrl : TlAbsMessageEntity
    {
        public override int Constructor => 1859134776;

        public int Offset { get; set; }
        public int Length { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Offset = br.ReadInt32();
            Length = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Offset);
            bw.Write(Length);
        }
    }
}