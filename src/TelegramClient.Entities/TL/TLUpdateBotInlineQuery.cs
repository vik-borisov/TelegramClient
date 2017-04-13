using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1417832080)]
    public class TlUpdateBotInlineQuery : TlAbsUpdate
    {
        public override int Constructor => 1417832080;

        public int Flags { get; set; }
        public long QueryId { get; set; }
        public int UserId { get; set; }
        public string Query { get; set; }
        public TlAbsGeoPoint Geo { get; set; }
        public string Offset { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Geo != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            QueryId = br.ReadInt64();
            UserId = br.ReadInt32();
            Query = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                Geo = (TlAbsGeoPoint) ObjectUtils.DeserializeObject(br);
            else
                Geo = null;

            Offset = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(QueryId);
            bw.Write(UserId);
            StringUtil.Serialize(Query, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(Geo, bw);
            StringUtil.Serialize(Offset, bw);
        }
    }
}