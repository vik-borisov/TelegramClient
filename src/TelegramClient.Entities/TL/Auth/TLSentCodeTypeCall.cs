using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1398007207)]
    public class TlSentCodeTypeCall : TlAbsSentCodeType
    {
        public override int Constructor => 1398007207;

        public int Length { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Length = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Length);
        }
    }
}