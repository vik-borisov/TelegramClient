using System.IO;

namespace TelegramClient.Entities.TL.Users
{
    [TlObject(-902781519)]
    public class TlRequestGetFullUser : TlMethod
    {
        public override int Constructor => -902781519;

        public TlAbsInputUser Id { get; set; }
        public TlUserFull Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlUserFull) ObjectUtils.DeserializeObject(br);
        }
    }
}