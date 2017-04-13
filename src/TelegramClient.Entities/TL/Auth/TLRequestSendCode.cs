using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-2035355412)]
    public class TlRequestSendCode : TlMethod
    {
        public override int Constructor => -2035355412;

        public int Flags { get; set; }
        public bool AllowFlashcall { get; set; }
        public string PhoneNumber { get; set; }
        public bool? CurrentNumber { get; set; }
        public int ApiId { get; set; }
        public string ApiHash { get; set; }
        public TlSentCode Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = AllowFlashcall ? Flags | 1 : Flags & ~1;
            Flags = CurrentNumber != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            AllowFlashcall = (Flags & 1) != 0;
            PhoneNumber = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                CurrentNumber = BoolUtil.Deserialize(br);
            else
                CurrentNumber = null;

            ApiId = br.ReadInt32();
            ApiHash = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            StringUtil.Serialize(PhoneNumber, bw);
            if ((Flags & 1) != 0)
                BoolUtil.Serialize(CurrentNumber.Value, bw);
            bw.Write(ApiId);
            StringUtil.Serialize(ApiHash, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlSentCode) ObjectUtils.DeserializeObject(br);
        }
    }
}