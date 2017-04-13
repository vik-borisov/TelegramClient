using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1995686519)]
    public class TlInputBotInlineMessageId : TlObject
    {
        public override int Constructor => -1995686519;

        public int DcId { get; set; }
        public long Id { get; set; }
        public long AccessHash { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            DcId = br.ReadInt32();
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(DcId);
            bw.Write(Id);
            bw.Write(AccessHash);
        }
    }
}