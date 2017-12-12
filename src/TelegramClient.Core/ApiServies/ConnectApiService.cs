namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using log4net;

    using OpenTl.Schema;
    using OpenTl.Schema.Help;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.Auth;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(IConnectApiService))]
    internal class ConnectApiService : IConnectApiService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectApiService));

        private TDcOption[] _dcOptions;

        public IClientSettings ClientSettings { get; set; }

        public ISessionStore SessionStore { get; set; }

        public IMtProtoPlainSender MtProtoPlainSender { get; set; }

        public IConfirmationSendService ConfirmationSendService { get; set; }

        public IRecievingService ProtoRecieveService { get; set; }

        public ISenderService SendService { get; set; }

        public Task ConnectAsync()
        {
            return ConnectAsync(false);
        }

        public Task ReAuthenticateAsync()
        {
            return ConnectAsync(true);
        }

        public async Task ReconnectToDcAsync(int dcId)
        {
            if (_dcOptions == null || !_dcOptions.Any())
            {
                throw new InvalidOperationException($"Can't reconnect. Establish initial connection first.");
            }

            var dc = _dcOptions.First(d => d.Id == dcId);

            ClientSettings.Session.ServerAddress = dc.IpAddress;
            ClientSettings.Session.Port = dc.Port;

            await ConnectAsync(true).ConfigureAwait(false);
        }

        private async Task ConnectAsync(bool forceAuth)
        {
            if (ClientSettings.Session.AuthKey == null || forceAuth)
            {
                var result = await DoAuthentication().ConfigureAwait(false);
                ClientSettings.Session.AuthKey = result.AuthKey;
                ClientSettings.Session.TimeOffset = result.TimeOffset;

                SessionStore.Save();
            }

            ProtoRecieveService.StartReceiving();

            //set-up layer
            var request = new RequestInvokeWithLayer
                          {
                              Layer = SchemaInfo.SchemaVersion,
                              Query = new RequestInitConnection
                                      {
                                          ApiId = ClientSettings.AppId,
                                          AppVersion = "1.0.0",
                                          DeviceModel = "PC",
                                          LangCode = "en",
                                          LangPack = "tdesktop",
                                          SystemLangCode = "en",
                                          Query = new RequestGetConfig(),
                                          SystemVersion = "Win 10.0"
                                      }
                          };

            var response = (TConfig)await SendService.SendRequestAsync(request).ConfigureAwait(false);
            _dcOptions = response.DcOptions.Items.Cast<TDcOption>().ToArray();
        }

        private async Task<Step3Response> DoAuthentication()
        {
            Log.Info("Try do authentication");

            var step1 = new Step1PqRequest();
            var step1Result = await MtProtoPlainSender.SendAndReceive(step1.ToBytes()).ConfigureAwait(false);
            var step1Response = step1.FromBytes(step1Result);

            Log.Debug("First step is done");

            var step2 = new Step2DhExchange();
            var step2Result = await MtProtoPlainSender.SendAndReceive(
                                  step2.ToBytes(
                                      step1Response.Nonce,
                                      step1Response.ServerNonce,
                                      step1Response.Fingerprints,
                                      step1Response.Pq)).ConfigureAwait(false);
            var step2Response = step2.FromBytes(step2Result);

            Log.Debug("Second step is done");

            var step3 = new Step3CompleteDhExchange();
            var step3Result = await MtProtoPlainSender.SendAndReceive(
                                                          step3.ToBytes(
                                                              step2Response.Nonce,
                                                              step2Response.ServerNonce,
                                                              step2Response.NewNonce,
                                                              step2Response.EncryptedAnswer))
                                                      .ConfigureAwait(false);
            var step3Response = step3.FromBytes(step3Result);

            Log.Debug("Third step is done");

            return step3Response;
        }
    }
}