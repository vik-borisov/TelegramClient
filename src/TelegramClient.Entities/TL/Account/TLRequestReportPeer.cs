using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-1374118561)]
    public class TlRequestReportPeer : TlMethod
    {
        public override int Constructor => -1374118561;

        public TlAbsInputPeer Peer { get; set; }
        public TlAbsReportReason Reason { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Reason = (TlAbsReportReason) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(Reason, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}