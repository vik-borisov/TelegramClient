namespace TelegramClient.Core.Responces
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using TelegramClient.Core.Helpers;

    public class ContainerItem
    {
        private readonly ulong _messageId;

        private readonly BaseResponce _responce;

        private readonly ulong _sequenceId;

        public ContainerItem(ulong messageId, ulong sequenceId, BaseResponce responce)
        {
            _messageId = messageId;
            _sequenceId = sequenceId;
            _responce = responce;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(_messageId);
            writer.Write(_sequenceId);

            var body = BinaryHelper.WriteBytes(_responce.Serialize);
            writer.Write(body.Length);
            writer.Write(body);
        }
    }

    public class ContainerResponce : BaseResponce
    {
        private readonly IEnumerable<ContainerItem> _items;

        public override uint Constructor { get; } = 0x73f1f8dc;

        public ContainerResponce(ulong requestMessageId, IEnumerable<ContainerItem> items) : base(requestMessageId)
        {
            _items = items;
        }

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(_items.Count());
            foreach (var item in _items)
            {
                item.Serialize(writer);
            }
        }
    }
}