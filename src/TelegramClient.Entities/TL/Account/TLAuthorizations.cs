using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(307276766)]
    public class TlAuthorizations : TlObject
    {
        public override int Constructor => 307276766;

        public TlVector<TlAuthorization> Authorizations { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Authorizations = ObjectUtils.DeserializeVector<TlAuthorization>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Authorizations, bw);
        }
    }
}