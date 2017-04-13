using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(623001124)]
    public class TlRequestGetWebPagePreview : TlMethod
    {
        public override int Constructor => 623001124;

        public string Message { get; set; }
        public TlAbsMessageMedia Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Message = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Message, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsMessageMedia) ObjectUtils.DeserializeObject(br);
        }
    }
}