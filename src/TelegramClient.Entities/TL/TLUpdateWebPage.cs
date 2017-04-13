using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2139689491)]
    public class TlUpdateWebPage : TlAbsUpdate
    {
        public override int Constructor => 2139689491;

        public TlAbsWebPage Webpage { get; set; }
        public int Pts { get; set; }
        public int PtsCount { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Webpage = (TlAbsWebPage) ObjectUtils.DeserializeObject(br);
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Webpage, bw);
            bw.Write(Pts);
            bw.Write(PtsCount);
        }
    }
}