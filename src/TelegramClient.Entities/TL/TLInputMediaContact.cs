using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1494984313)]
    public class TlInputMediaContact : TlAbsInputMedia
    {
        public override int Constructor => -1494984313;

        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumber = StringUtil.Deserialize(br);
            FirstName = StringUtil.Deserialize(br);
            LastName = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(PhoneNumber, bw);
            StringUtil.Serialize(FirstName, bw);
            StringUtil.Serialize(LastName, bw);
        }
    }
}