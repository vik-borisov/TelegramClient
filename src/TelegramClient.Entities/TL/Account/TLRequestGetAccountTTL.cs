using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(150761757)]
    public class TlRequestGetAccountTtl : TlMethod
    {
        public override int Constructor => 150761757;

        public TlAccountDaysTtl Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAccountDaysTtl) ObjectUtils.DeserializeObject(br);
        }
    }
}