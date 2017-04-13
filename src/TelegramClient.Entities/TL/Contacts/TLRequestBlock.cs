using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(858475004)]
    public class TlRequestBlock : TlMethod
    {
        public override int Constructor => 858475004;

        public TlAbsInputUser Id { get; set; }
        public bool Response { get; set; }


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
            Response = BoolUtil.Deserialize(br);
        }
    }
}