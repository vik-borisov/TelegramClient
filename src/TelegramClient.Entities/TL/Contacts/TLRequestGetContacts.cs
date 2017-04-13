using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(583445000)]
    public class TlRequestGetContacts : TlMethod
    {
        public override int Constructor => 583445000;

        public string Hash { get; set; }
        public TlAbsContacts Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Hash, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsContacts) ObjectUtils.DeserializeObject(br);
        }
    }
}