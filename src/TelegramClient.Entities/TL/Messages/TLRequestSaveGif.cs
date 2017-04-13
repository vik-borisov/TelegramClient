using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(846868683)]
    public class TlRequestSaveGif : TlMethod
    {
        public override int Constructor => 846868683;

        public TlAbsInputDocument Id { get; set; }
        public bool Unsave { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputDocument) ObjectUtils.DeserializeObject(br);
            Unsave = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
            BoolUtil.Serialize(Unsave, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}