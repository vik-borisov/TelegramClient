using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-208488460)]
    public class TlInputPhoneContact : TlObject
    {
        public override int Constructor => -208488460;

        public long ClientId { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ClientId = br.ReadInt64();
            Phone = StringUtil.Deserialize(br);
            FirstName = StringUtil.Deserialize(br);
            LastName = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ClientId);
            StringUtil.Serialize(Phone, bw);
            StringUtil.Serialize(FirstName, bw);
            StringUtil.Serialize(LastName, bw);
        }
    }
}