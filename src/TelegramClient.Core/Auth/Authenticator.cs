using System.Threading.Tasks;
using TelegramClient.Core.Network;

namespace TelegramClient.Core.Auth
{
    public static class Authenticator
    {
        public static async Task<Step3Response> DoAuthentication(TcpTransport transport)
        {
            var sender = new MtProtoPlainSender(transport);
            var step1 = new Step1PqRequest();

            await sender.Send(step1.ToBytes());
            var step1Response = step1.FromBytes(await sender.Receive());

            var step2 = new Step2DhExchange();
            await sender.Send(step2.ToBytes(
                step1Response.Nonce,
                step1Response.ServerNonce,
                step1Response.Fingerprints,
                step1Response.Pq));

            var step2Response = step2.FromBytes(await sender.Receive());

            var step3 = new Step3CompleteDhExchange();
            await sender.Send(step3.ToBytes(
                step2Response.Nonce,
                step2Response.ServerNonce,
                step2Response.NewNonce,
                step2Response.EncryptedAnswer));

            var step3Response = step3.FromBytes(await sender.Receive());

            return step3Response;
        }
    }
}