using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-981018084)]
    public class TlWebPagePending : TlAbsWebPage
    {
        public override int Constructor => -981018084;

        public long Id { get; set; }
        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(Date);
        }
    }
}