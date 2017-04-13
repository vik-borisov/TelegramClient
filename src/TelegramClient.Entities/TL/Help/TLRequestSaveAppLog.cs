using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(1862465352)]
    public class TlRequestSaveAppLog : TlMethod
    {
        public override int Constructor => 1862465352;

        public TlVector<TlInputAppEvent> Events { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Events = ObjectUtils.DeserializeVector<TlInputAppEvent>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Events, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}