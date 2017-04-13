using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(1504393374)]
    public class TlRequestDeleteContacts : TlMethod
    {
        public override int Constructor => 1504393374;

        public TlVector<TlAbsInputUser> Id { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = ObjectUtils.DeserializeVector<TlAbsInputUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}