using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-934882771)]
    public class TlRequestExportMessageLink : TlMethod
    {
        public override int Constructor => -934882771;

        public TlAbsInputChannel Channel { get; set; }
        public int Id { get; set; }
        public TlExportedMessageLink Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Id = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            bw.Write(Id);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlExportedMessageLink) ObjectUtils.DeserializeObject(br);
        }
    }
}