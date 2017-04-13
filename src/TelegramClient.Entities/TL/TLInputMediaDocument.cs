using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(444068508)]
    public class TlInputMediaDocument : TlAbsInputMedia
    {
        public override int Constructor => 444068508;

        public TlAbsInputDocument Id { get; set; }
        public string Caption { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputDocument) ObjectUtils.DeserializeObject(br);
            Caption = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
            StringUtil.Serialize(Caption, bw);
        }
    }
}