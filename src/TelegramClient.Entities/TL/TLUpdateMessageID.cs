using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1318109142)]
    public class TlUpdateMessageId : TlAbsUpdate
    {
        public override int Constructor => 1318109142;

        public int Id { get; set; }
        public long RandomId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            RandomId = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(RandomId);
        }
    }
}