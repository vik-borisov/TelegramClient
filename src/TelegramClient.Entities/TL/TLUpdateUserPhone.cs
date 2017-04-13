using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(314130811)]
    public class TlUpdateUserPhone : TlAbsUpdate
    {
        public override int Constructor => 314130811;

        public int UserId { get; set; }
        public string Phone { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Phone = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            StringUtil.Serialize(Phone, bw);
        }
    }
}