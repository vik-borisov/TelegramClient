using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1058912715)]
    public class TlDhConfigNotModified : TlAbsDhConfig
    {
        public override int Constructor => -1058912715;

        public byte[] Random { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Random = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BytesUtil.Serialize(Random, bw);
        }
    }
}