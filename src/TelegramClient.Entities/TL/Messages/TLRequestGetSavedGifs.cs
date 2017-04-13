using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-2084618926)]
    public class TlRequestGetSavedGifs : TlMethod
    {
        public override int Constructor => -2084618926;

        public int Hash { get; set; }
        public TlAbsSavedGifs Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsSavedGifs) ObjectUtils.DeserializeObject(br);
        }
    }
}