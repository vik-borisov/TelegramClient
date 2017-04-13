using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(1340184318)]
    public class TlRequestImportCard : TlMethod
    {
        public override int Constructor => 1340184318;

        public TlVector<int> ExportCard { get; set; }
        public TlAbsUser Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ExportCard = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(ExportCard, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUser) ObjectUtils.DeserializeObject(br);
        }
    }
}