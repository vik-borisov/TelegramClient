using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1551583367)]
    public class TlReceivedNotifyMessage : TlObject
    {
        public override int Constructor => -1551583367;

        public int Id { get; set; }
        public int Flags { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            Flags = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Id);
        }
    }
}