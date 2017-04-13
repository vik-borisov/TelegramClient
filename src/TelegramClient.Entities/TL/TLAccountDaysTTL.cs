using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1194283041)]
    public class TlAccountDaysTtl : TlObject
    {
        public override int Constructor => -1194283041;

        public int Days { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Days = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Days);
        }
    }
}