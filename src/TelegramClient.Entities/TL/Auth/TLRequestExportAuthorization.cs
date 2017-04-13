using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-440401971)]
    public class TlRequestExportAuthorization : TlMethod
    {
        public override int Constructor => -440401971;

        public int DcId { get; set; }
        public TlExportedAuthorization Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            DcId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(DcId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlExportedAuthorization) ObjectUtils.DeserializeObject(br);
        }
    }
}