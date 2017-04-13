using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-1131605573)]
    public class TlRequestGetPasswordSettings : TlMethod
    {
        public override int Constructor => -1131605573;

        public byte[] CurrentPasswordHash { get; set; }
        public TlPasswordSettings Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            CurrentPasswordHash = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BytesUtil.Serialize(CurrentPasswordHash, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlPasswordSettings) ObjectUtils.DeserializeObject(br);
        }
    }
}