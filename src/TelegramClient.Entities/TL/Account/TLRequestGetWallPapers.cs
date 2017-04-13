using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-1068696894)]
    public class TlRequestGetWallPapers : TlMethod
    {
        public override int Constructor => -1068696894;

        public TlVector<TlAbsWallPaper> Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = ObjectUtils.DeserializeVector<TlAbsWallPaper>(br);
        }
    }
}