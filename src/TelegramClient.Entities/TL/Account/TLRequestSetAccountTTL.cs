using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(608323678)]
    public class TlRequestSetAccountTtl : TlMethod
    {
        public override int Constructor => 608323678;

        public TlAccountDaysTtl Ttl { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Ttl = (TlAccountDaysTtl) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Ttl, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}