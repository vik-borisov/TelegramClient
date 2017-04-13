using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1444661369)]
    public class TlContactBlocked : TlObject
    {
        public override int Constructor => 1444661369;

        public int UserId { get; set; }
        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            bw.Write(Date);
        }
    }
}