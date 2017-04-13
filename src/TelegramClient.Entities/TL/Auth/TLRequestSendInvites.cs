using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1998331287)]
    public class TlRequestSendInvites : TlMethod
    {
        public override int Constructor => 1998331287;

        public TlVector<string> PhoneNumbers { get; set; }
        public string Message { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PhoneNumbers = ObjectUtils.DeserializeVector<string>(br);
            Message = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(PhoneNumbers, bw);
            StringUtil.Serialize(Message, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}