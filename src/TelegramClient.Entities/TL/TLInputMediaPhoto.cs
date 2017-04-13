using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-373312269)]
    public class TlInputMediaPhoto : TlAbsInputMedia
    {
        public override int Constructor => -373312269;

        public TlAbsInputPhoto Id { get; set; }
        public string Caption { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputPhoto) ObjectUtils.DeserializeObject(br);
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