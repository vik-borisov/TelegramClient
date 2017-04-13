using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-1907842680)]
    public class TlRequestDropTempAuthKeys : TlMethod
    {
        public override int Constructor => -1907842680;

        public TlVector<long> ExceptAuthKeys { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ExceptAuthKeys = ObjectUtils.DeserializeVector<long>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(ExceptAuthKeys, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}