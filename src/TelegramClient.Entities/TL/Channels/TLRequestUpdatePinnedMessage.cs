using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-1490162350)]
    public class TlRequestUpdatePinnedMessage : TlMethod
    {
        public override int Constructor => -1490162350;

        public int Flags { get; set; }
        public bool Silent { get; set; }
        public TlAbsInputChannel Channel { get; set; }
        public int Id { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Silent ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Silent = (Flags & 1) != 0;
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Id = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Channel, bw);
            bw.Write(Id);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}