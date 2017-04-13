using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-350980120)]
    public class TlWebPageEmpty : TlAbsWebPage
    {
        public override int Constructor => -350980120;

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