using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(1567990072)]
    public class TlDifferenceEmpty : TlAbsDifference
    {
        public override int Constructor => 1567990072;

        public int Date { get; set; }
        public int Seq { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Date = br.ReadInt32();
            Seq = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Date);
            bw.Write(Seq);
        }
    }
}