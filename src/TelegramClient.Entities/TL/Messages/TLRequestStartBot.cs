using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-421563528)]
    public class TlRequestStartBot : TlMethod
    {
        public override int Constructor => -421563528;

        public TlAbsInputUser Bot { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public long RandomId { get; set; }
        public string StartParam { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Bot = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            RandomId = br.ReadInt64();
            StartParam = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Bot, bw);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(RandomId);
            StringUtil.Serialize(StartParam, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}