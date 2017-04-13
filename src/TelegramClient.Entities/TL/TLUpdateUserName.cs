using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1489818765)]
    public class TlUpdateUserName : TlAbsUpdate
    {
        public override int Constructor => -1489818765;

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            FirstName = StringUtil.Deserialize(br);
            LastName = StringUtil.Deserialize(br);
            Username = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            StringUtil.Serialize(FirstName, bw);
            StringUtil.Serialize(LastName, bw);
            StringUtil.Serialize(Username, bw);
        }
    }
}