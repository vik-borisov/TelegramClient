using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(168039573)]
    public class TlRequestGetDifference : TlMethod
    {
        public override int Constructor => 168039573;

        public int Pts { get; set; }
        public int Date { get; set; }
        public int Qts { get; set; }
        public TlAbsDifference Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Pts = br.ReadInt32();
            Date = br.ReadInt32();
            Qts = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Pts);
            bw.Write(Date);
            bw.Write(Qts);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsDifference) ObjectUtils.DeserializeObject(br);
        }
    }
}