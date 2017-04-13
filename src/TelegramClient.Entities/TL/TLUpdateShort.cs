using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2027216577)]
    public class TlUpdateShort : TlAbsUpdates
    {
        public override int Constructor => 2027216577;

        public TlAbsUpdate Update { get; set; }
        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Update = (TlAbsUpdate) ObjectUtils.DeserializeObject(br);
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Update, bw);
            bw.Write(Date);
        }
    }
}