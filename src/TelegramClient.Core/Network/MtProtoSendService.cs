using System.IO;

using TelegramClient.Core.MTProto.Crypto;
using TelegramClient.Core.Utils;
using TelegramClient.Entities;

namespace TelegramClient.Core.Network
{
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.Helpers;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Settings;

    internal class MtProtoSendService : IMtProtoSender
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(MtProtoSendService));

        public ITcpTransport TcpTransport { get; set; }

		public IClientSettings ClientSettings { get; set; }

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

	    private byte[] PrepareToSend(TlMethod request)
	    {
		    var packet = BinaryHelper.WriteBytes(request.SerializeBody);

            request.MessageId = ClientSettings.Session.GetNewMessageId();

            Log.Debug($"Send message with Id = {request.MessageId}");

			byte[] msgKey;
			byte[] ciphertext;
			using (var plaintextPacket = MakeMemory(8 + 8 + 8 + 4 + 4 + packet.Length))
			{
				using (var plaintextWriter = new BinaryWriter(plaintextPacket))
				{
					plaintextWriter.Write(ClientSettings.Session.Salt);
					plaintextWriter.Write(ClientSettings.Session.Id);
					plaintextWriter.Write(request.MessageId);
					plaintextWriter.Write(ClientSettings.Session.GenerateSessionSeqNo(request.Confirmed));
					plaintextWriter.Write(packet.Length);
					plaintextWriter.Write(packet);

					plaintextPacket.TryGetBuffer(out var buffer);
					msgKey = TlHelpers.CalcMsgKey(buffer.Array);
					ciphertext = AES.EncryptAes(TlHelpers.CalcKey(ClientSettings.Session.AuthKey.Data, msgKey, true), buffer.Array);
				}
			}

			using (var ciphertextPacket = MakeMemory(8 + 16 + ciphertext.Length))
			{
				using (var writer = new BinaryWriter(ciphertextPacket))
				{
					writer.Write(ClientSettings.Session.AuthKey.Id);
					writer.Write(msgKey);
					writer.Write(ciphertext);

					return ciphertextPacket.ToArray();
				}
			}
		}

	    public async Task Send(TlMethod request)
	    {
	        var preparedData = PrepareToSend(request);

	        TcpTransport.Send(preparedData);

            await ConfirmationRecieveService.WaitForConfirm(request.MessageId);
	    }

	    private MemoryStream MakeMemory(int len)
		{
			return new MemoryStream(new byte[len], 0, len, true, true);
		}
	}
}