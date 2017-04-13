using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1906403213)]
    public class TlUpdateDcOptions : TlAbsUpdate
    {
        public override int Constructor => -1906403213;

        public TlVector<TlDcOption> DcOptions { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            DcOptions = ObjectUtils.DeserializeVector<TlDcOption>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(DcOptions, bw);
        }
    }
}