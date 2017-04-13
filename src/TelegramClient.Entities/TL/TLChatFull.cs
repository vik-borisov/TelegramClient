using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(771925524)]
    public class TlChatFull : TlAbsChatFull
    {
        public override int Constructor => 771925524;

        public int Id { get; set; }
        public TlAbsChatParticipants Participants { get; set; }
        public TlAbsPhoto ChatPhoto { get; set; }
        public TlAbsPeerNotifySettings NotifySettings { get; set; }
        public TlAbsExportedChatInvite ExportedInvite { get; set; }
        public TlVector<TlBotInfo> BotInfo { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            Participants = (TlAbsChatParticipants) ObjectUtils.DeserializeObject(br);
            ChatPhoto = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
            NotifySettings = (TlAbsPeerNotifySettings) ObjectUtils.DeserializeObject(br);
            ExportedInvite = (TlAbsExportedChatInvite) ObjectUtils.DeserializeObject(br);
            BotInfo = ObjectUtils.DeserializeVector<TlBotInfo>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            ObjectUtils.SerializeObject(Participants, bw);
            ObjectUtils.SerializeObject(ChatPhoto, bw);
            ObjectUtils.SerializeObject(NotifySettings, bw);
            ObjectUtils.SerializeObject(ExportedInvite, bw);
            ObjectUtils.SerializeObject(BotInfo, bw);
        }
    }
}