namespace TelegramClient.Core.Responces
{
    using System.IO;

    using TelegramClient.Entities;

    public class RpcResponce: BaseResponce
    {
        private readonly TlObject _rpcResult;

        public override uint Constructor { get; } = 0xf35c6d01;

        public RpcResponce(ulong requestMessageId, TlObject rpcResult) : base(requestMessageId)
        {
            _rpcResult = rpcResult;
        }

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(_rpcResult.Serialize());
        }
    }
}