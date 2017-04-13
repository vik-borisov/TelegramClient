using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-2066640507)]
    public class TlAffectedMessages : TlObject
    {
        public override int Constructor => -2066640507;

        public int Pts { get; set; }
        public int PtsCount { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Pts);
            bw.Write(PtsCount);
        }
    }
}