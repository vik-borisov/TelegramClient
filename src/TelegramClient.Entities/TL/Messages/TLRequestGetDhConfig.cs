using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(651135312)]
    public class TlRequestGetDhConfig : TlMethod
    {
        public override int Constructor => 651135312;

        public int Version { get; set; }
        public int RandomLength { get; set; }
        public TlAbsDhConfig Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Version = br.ReadInt32();
            RandomLength = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Version);
            bw.Write(RandomLength);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsDhConfig) ObjectUtils.DeserializeObject(br);
        }
    }
}