using System.IO;

namespace TelegramClient.Entities.TL.Users
{
    [TlObject(227648840)]
    public class TlRequestGetUsers : TlMethod
    {
        public override int Constructor => 227648840;

        public TlVector<TlAbsInputUser> Id { get; set; }
        public TlVector<TlAbsUser> Response { get; set; }


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
            Response = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }
    }
}