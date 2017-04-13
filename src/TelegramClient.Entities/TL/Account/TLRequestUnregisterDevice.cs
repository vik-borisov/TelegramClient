using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(1707432768)]
    public class TlRequestUnregisterDevice : TlMethod
    {
        public override int Constructor => 1707432768;

        public int TokenType { get; set; }
        public string Token { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            TokenType = br.ReadInt32();
            Token = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(TokenType);
            StringUtil.Serialize(Token, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}