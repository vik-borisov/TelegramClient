using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1933187430)]
    public class TlChannelParticipantKicked : TlAbsChannelParticipant
    {
        public override int Constructor => -1933187430;

        public int UserId { get; set; }
        public int KickedBy { get; set; }
        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            KickedBy = br.ReadInt32();
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            bw.Write(KickedBy);
            bw.Write(Date);
        }
    }
}