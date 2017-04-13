using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(537022650)]
    public class TlUserEmpty : TlAbsUser
    {
        public override int Constructor => 537022650;

        public int Id { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
        }
    }
}