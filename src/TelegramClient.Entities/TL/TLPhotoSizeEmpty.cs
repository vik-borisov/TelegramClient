using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(236446268)]
    public class TlPhotoSizeEmpty : TlAbsPhotoSize
    {
        public override int Constructor => 236446268;

        public string Type { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Type = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Type, bw);
        }
    }
}