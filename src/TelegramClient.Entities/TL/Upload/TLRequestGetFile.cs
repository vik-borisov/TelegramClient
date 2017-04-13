using System.IO;

namespace TelegramClient.Entities.TL.Upload
{
    [TlObject(-475607115)]
    public class TlRequestGetFile : TlMethod
    {
        public override int Constructor => -475607115;

        public TlAbsInputFileLocation Location { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public TlFile Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Location = (TlAbsInputFileLocation) ObjectUtils.DeserializeObject(br);
            Offset = br.ReadInt32();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Location, bw);
            bw.Write(Offset);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlFile) ObjectUtils.DeserializeObject(br);
        }
    }
}