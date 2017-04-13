using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-471670279)]
    public class TlChannelParticipantCreator : TlAbsChannelParticipant
    {
        public override int Constructor => -471670279;

        public int UserId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
        }
    }
}