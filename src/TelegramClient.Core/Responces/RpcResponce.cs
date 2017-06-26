namespace TelegramClient.Core.Responces
{
    using System.IO;

    using OpenTl.Schema;
    using OpenTl.Schema.Serialization;

    public class RpcResponce: BaseResponce
    {
        private readonly IObject _rpcResult;

        public override uint Constructor { get; } = 0xf35c6d01;

        public RpcResponce(ulong requestMessageId, IObject rpcResult) : base(requestMessageId)
        {
            _rpcResult = rpcResult;
        }

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(Serializer.SerializeObject(_rpcResult).ToArray());
        }
    }
}