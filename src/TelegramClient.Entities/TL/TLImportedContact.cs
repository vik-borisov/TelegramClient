using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-805141448)]
    public class TlImportedContact : TlObject
    {
        public override int Constructor => -805141448;

        public int UserId { get; set; }
        public long ClientId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            ClientId = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            bw.Write(ClientId);
        }
    }
}