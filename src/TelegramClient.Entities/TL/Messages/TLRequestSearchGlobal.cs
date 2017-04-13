using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1640190800)]
    public class TlRequestSearchGlobal : TlMethod
    {
        public override int Constructor => -1640190800;

        public string Q { get; set; }
        public int OffsetDate { get; set; }
        public TlAbsInputPeer OffsetPeer { get; set; }
        public int OffsetId { get; set; }
        public int Limit { get; set; }
        public TlAbsMessages Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Q = StringUtil.Deserialize(br);
            OffsetDate = br.ReadInt32();
            OffsetPeer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            OffsetId = br.ReadInt32();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Q, bw);
            bw.Write(OffsetDate);
            ObjectUtils.SerializeObject(OffsetPeer, bw);
            bw.Write(OffsetId);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}