using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(363700068)]
    public class TlRequestSetInlineGameScore : TlMethod
    {
        public override int Constructor => 363700068;

        public int Flags { get; set; }
        public bool EditMessage { get; set; }
        public TlInputBotInlineMessageId Id { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public int Score { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = EditMessage ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            EditMessage = (Flags & 1) != 0;
            Id = (TlInputBotInlineMessageId) ObjectUtils.DeserializeObject(br);
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Score = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Id, bw);
            ObjectUtils.SerializeObject(UserId, bw);
            bw.Write(Score);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}