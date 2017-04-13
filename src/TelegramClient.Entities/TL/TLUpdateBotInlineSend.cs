using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(239663460)]
    public class TlUpdateBotInlineSend : TlAbsUpdate
    {
        public override int Constructor => 239663460;

        public int Flags { get; set; }
        public int UserId { get; set; }
        public string Query { get; set; }
        public TlAbsGeoPoint Geo { get; set; }
        public string Id { get; set; }
        public TlInputBotInlineMessageId MsgId { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Geo != null ? Flags | 1 : Flags & ~1;
            Flags = MsgId != null ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            UserId = br.ReadInt32();
            Query = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                Geo = (TlAbsGeoPoint) ObjectUtils.DeserializeObject(br);
            else
                Geo = null;

            Id = StringUtil.Deserialize(br);
            if ((Flags & 2) != 0)
                MsgId = (TlInputBotInlineMessageId) ObjectUtils.DeserializeObject(br);
            else
                MsgId = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(UserId);
            StringUtil.Serialize(Query, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(Geo, bw);
            StringUtil.Serialize(Id, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(MsgId, bw);
        }
    }
}