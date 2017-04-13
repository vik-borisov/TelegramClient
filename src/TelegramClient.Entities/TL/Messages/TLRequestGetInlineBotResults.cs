using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1364105629)]
    public class TlRequestGetInlineBotResults : TlMethod
    {
        public override int Constructor => 1364105629;

        public int Flags { get; set; }
        public TlAbsInputUser Bot { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public TlAbsInputGeoPoint GeoPoint { get; set; }
        public string Query { get; set; }
        public string Offset { get; set; }
        public TlBotResults Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = GeoPoint != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Bot = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                GeoPoint = (TlAbsInputGeoPoint) ObjectUtils.DeserializeObject(br);
            else
                GeoPoint = null;

            Query = StringUtil.Deserialize(br);
            Offset = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Bot, bw);
            ObjectUtils.SerializeObject(Peer, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(GeoPoint, bw);
            StringUtil.Serialize(Query, bw);
            StringUtil.Serialize(Offset, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlBotResults) ObjectUtils.DeserializeObject(br);
        }
    }
}