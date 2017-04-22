using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TelegramClient.Core.Auth;
using TelegramClient.Core.Network;
using TelegramClient.Core.Utils;
using TelegramClient.Entities;
using TelegramClient.Entities.TL;
using TelegramClient.Entities.TL.Help;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Core
{
    using LightInject;

    using PommaLabs.Thrower.Logging;

    using TelegramClient.Core.Settings;

    using LogLevel = PommaLabs.Thrower.Logging.LogLevel;

    internal class Client : ITelegramClient
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(Client));

        public IClientSettings ClientSettings { get; set; }

        public IServiceContainer Container { get; set; }

        public IMtProtoSender Sender { get; set; }

        public IMtProtoPlainSender MtProtoPlainSender { get; set; }

        private List<TlDcOption> _dcOptions;

        private async Task<Step3Response> DoAuthentication()
        {
            var step1 = new Step1PqRequest();
            var step1Result = await MtProtoPlainSender.SendAndReceive(step1.ToBytes());
            var step1Response = step1.FromBytes(step1Result);

            var step2 = new Step2DhExchange();
            var step2Result = await MtProtoPlainSender.SendAndReceive(step2.ToBytes(
                                  step1Response.Nonce,
                                  step1Response.ServerNonce,
                                  step1Response.Fingerprints,
                                  step1Response.Pq));
            var step2Response = step2.FromBytes(step2Result);

            var step3 = new Step3CompleteDhExchange();
            var step3Result = await MtProtoPlainSender.SendAndReceive(step3.ToBytes(
                step2Response.Nonce,
                step2Response.ServerNonce,
                step2Response.NewNonce,
                step2Response.EncryptedAnswer));
            var step3Response = step3.FromBytes(step3Result);
            return step3Response;
        }

        public async Task ConnectAsync(bool reconnect = false)
        {
            if (ClientSettings.Session.AuthKey == null || reconnect)
            {
                var result = await DoAuthentication();
                ClientSettings.Session.AuthKey = result.AuthKey;
                ClientSettings.Session.TimeOffset = result.TimeOffset;
            }

            //set-up layer
            var config = new TlRequestGetConfig();
            var request = new TlRequestInitConnection
            {
                ApiId = ClientSettings.AppId,
                AppVersion = "1.0.0",
                DeviceModel = "PC",
                LangCode = "en",
                Query = config,
                SystemVersion = "Win 10.0"
            };
            var invokewithLayer = new TlRequestInvokeWithLayer {Layer = 57, Query = request};

            var response = await SendRequestAsync<TlConfig>(invokewithLayer);
            _dcOptions = response.DcOptions.Lists;
        }

        public async Task ReconnectToDcAsync(int dcId)
        {
            if (_dcOptions == null || !_dcOptions.Any())
                throw new InvalidOperationException($"Can't reconnect. Establish initial connection first.");

            var dc = _dcOptions.First(d => d.Id == dcId);

            ClientSettings.Session.ServerAddress = dc.IpAddress;
            ClientSettings.Session.Port = dc.Port;

            await ConnectAsync(true);
        }

        public async Task<T> SendRequestAsync<T>(TlMethod methodToExecute)
        {
            Log.Log(LogLevel.Debug, () => $"Send message of type {methodToExecute}");

            await Sender.SendAndRecive(methodToExecute);
            return (T) methodToExecute.GetType().GetProperty("Response").GetValue(methodToExecute);
        }

        public async Task<TlAbsUpdates> SendMessageAsync(TlAbsInputPeer peer, string message)
        {
            if (!this.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            return await SendRequestAsync<TlAbsUpdates>(
                new TlRequestSendMessage
                {
                    Peer = peer,
                    Message = message,
                    RandomId = Helpers.GenerateRandomLong()
                });
        }

        public async Task SendPingAsync()
        {
            await Sender.SendPingAsync();
        }

        public void Dispose()
        {
            Container?.Dispose();
        }
    }
}