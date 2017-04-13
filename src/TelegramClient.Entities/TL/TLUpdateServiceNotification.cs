using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(942527460)]
    public class TlUpdateServiceNotification : TlAbsUpdate
    {
        public override int Constructor => 942527460;

        public string Type { get; set; }
        public string Message { get; set; }
        public TlAbsMessageMedia Media { get; set; }
        public bool Popup { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Type = StringUtil.Deserialize(br);
            Message = StringUtil.Deserialize(br);
            Media = (TlAbsMessageMedia) ObjectUtils.DeserializeObject(br);
            Popup = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Type, bw);
            StringUtil.Serialize(Message, bw);
            ObjectUtils.SerializeObject(Media, bw);
            BoolUtil.Serialize(Popup, bw);
        }
    }
}