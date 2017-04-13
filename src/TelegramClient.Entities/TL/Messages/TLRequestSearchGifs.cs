using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1080395925)]
    public class TlRequestSearchGifs : TlMethod
    {
        public override int Constructor => -1080395925;

        public string Q { get; set; }
        public int Offset { get; set; }
        public TlFoundGifs Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Q = StringUtil.Deserialize(br);
            Offset = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Q, bw);
            bw.Write(Offset);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlFoundGifs) ObjectUtils.DeserializeObject(br);
        }
    }
}