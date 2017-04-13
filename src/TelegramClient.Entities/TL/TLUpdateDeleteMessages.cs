using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1576161051)]
    public class TlUpdateDeleteMessages : TlAbsUpdate
    {
        public override int Constructor => -1576161051;

        public TlVector<int> Messages { get; set; }
        public int Pts { get; set; }
        public int PtsCount { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Messages = ObjectUtils.DeserializeVector<int>(br);
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Messages, bw);
            bw.Write(Pts);
            bw.Write(PtsCount);
        }
    }
}