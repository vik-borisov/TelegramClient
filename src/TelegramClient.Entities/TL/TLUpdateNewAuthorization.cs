using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1895411046)]
    public class TlUpdateNewAuthorization : TlAbsUpdate
    {
        public override int Constructor => -1895411046;

        public long AuthKeyId { get; set; }
        public int Date { get; set; }
        public string Device { get; set; }
        public string Location { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            AuthKeyId = br.ReadInt64();
            Date = br.ReadInt32();
            Device = StringUtil.Deserialize(br);
            Location = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(AuthKeyId);
            bw.Write(Date);
            StringUtil.Serialize(Device, bw);
            StringUtil.Serialize(Location, bw);
        }
    }
}