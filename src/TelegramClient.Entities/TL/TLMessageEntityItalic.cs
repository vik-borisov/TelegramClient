using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-2106619040)]
    public class TlMessageEntityItalic : TlAbsMessageEntity
    {
        public override int Constructor => -2106619040;

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