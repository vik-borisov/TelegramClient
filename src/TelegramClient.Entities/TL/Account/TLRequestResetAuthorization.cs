using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-545786948)]
    public class TlRequestResetAuthorization : TlMethod
    {
        public override int Constructor => -545786948;

        public long Hash { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}