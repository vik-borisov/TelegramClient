using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-2065352905)]
    public class TlRequestExportCard : TlMethod
    {
        public override int Constructor => -2065352905;

        public TlVector<int> Response { get; set; }


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
            Response = ObjectUtils.DeserializeVector<int>(br);
        }
    }
}