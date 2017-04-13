using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-1073693790)]
    public class TlSentCodeTypeSms : TlAbsSentCodeType
    {
        public override int Constructor => -1073693790;

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