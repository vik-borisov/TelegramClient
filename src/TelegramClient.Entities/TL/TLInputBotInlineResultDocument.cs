using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-459324)]
    public class TlInputBotInlineResultDocument : TlAbsInputBotInlineResult
    {
        public override int Constructor => -459324;

        public int Flags { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TlAbsInputDocument Document { get; set; }
        public TlAbsInputBotInlineMessage SendMessage { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Title != null ? Flags | 2 : Flags & ~2;
            Flags = Description != null ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Id = StringUtil.Deserialize(br);
            Type = StringUtil.Deserialize(br);
            if ((Flags & 2) != 0)
                Title = StringUtil.Deserialize(br);
            else
                Title = null;

            if ((Flags & 4) != 0)
                Description = StringUtil.Deserialize(br);
            else
                Description = null;

            Document = (TlAbsInputDocument) ObjectUtils.DeserializeObject(br);
            SendMessage = (TlAbsInputBotInlineMessage) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            StringUtil.Serialize(Id, bw);
            StringUtil.Serialize(Type, bw);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(Title, bw);
            if ((Flags & 4) != 0)
                StringUtil.Serialize(Description, bw);
            ObjectUtils.SerializeObject(Document, bw);
            ObjectUtils.SerializeObject(SendMessage, bw);
        }
    }
}