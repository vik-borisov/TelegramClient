using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(740433629)]
    public class TlDhConfig : TlAbsDhConfig
    {
        public override int Constructor => 740433629;

        public int G { get; set; }
        public byte[] P { get; set; }
        public int Version { get; set; }
        public byte[] Random { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            G = br.ReadInt32();
            P = BytesUtil.Deserialize(br);
            Version = br.ReadInt32();
            Random = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(G);
            BytesUtil.Serialize(P, bw);
            bw.Write(Version);
            BytesUtil.Serialize(Random, bw);
        }
    }
}