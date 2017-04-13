using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-1902823612)]
    public class TlRequestDeleteContact : TlMethod
    {
        public override int Constructor => -1902823612;

        public TlAbsInputUser Id { get; set; }
        public TlLink Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlLink) ObjectUtils.DeserializeObject(br);
        }
    }
}