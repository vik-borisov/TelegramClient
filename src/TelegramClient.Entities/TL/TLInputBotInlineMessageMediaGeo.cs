using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-190472735)]
    public class TlInputBotInlineMessageMediaGeo : TlAbsInputBotInlineMessage
    {
        public override int Constructor => -190472735;

        public int Flags { get; set; }
        public TlAbsInputGeoPoint GeoPoint { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            GeoPoint = (TlAbsInputGeoPoint) ObjectUtils.DeserializeObject(br);
            if ((Flags & 4) != 0)
                ReplyMarkup = (TlAbsReplyMarkup) ObjectUtils.DeserializeObject(br);
            else
                ReplyMarkup = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(GeoPoint, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
        }
    }
}