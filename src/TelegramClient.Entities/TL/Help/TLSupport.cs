using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(398898678)]
    public class TlSupport : TlObject
    {
        public override int Constructor => 398898678;

        public string PhoneNumber { get; set; }
        public TlAbsUser User { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumber = StringUtil.Deserialize(br);
            User = (TlAbsUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(PhoneNumber, bw);
            ObjectUtils.SerializeObject(User, bw);
        }
    }
}