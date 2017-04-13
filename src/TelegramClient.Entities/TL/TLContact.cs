using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-116274796)]
    public class TlContact : TlObject
    {
        public override int Constructor => -116274796;

        public int UserId { get; set; }
        public bool Mutual { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Mutual = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            BoolUtil.Serialize(Mutual, bw);
        }
    }
}