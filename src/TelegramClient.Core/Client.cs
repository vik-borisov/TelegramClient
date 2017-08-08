using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using TelegramClient.Core.Utils;

namespace TelegramClient.Core
{
    using System.IO;

    using log4net;

    using Autofac;

    using OpenTl.Common.Auth.Client;
    using OpenTl.Schema;
    using OpenTl.Schema.Help;
    using OpenTl.Schema.Messages;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.ApiServies;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(ITelegramClient))]
    internal class Client : ITelegramClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));

        public IClientSettings ClientSettings { get; set; }

        public IComponentContext Container { get; set; }

        public IMtProtoSender Sender { get; set; }

        public IConfirmationSendService ConfirmationSendService { get; set; }

        public IRecievingService ProtoRecieveService { get; set; }

        public IResponseResultGetter ResponseResultGetter { get; set; }

        public IMtProtoPlainSender MtProtoPlainSender { get; set; }

        public ISessionStore SessionStore { get; set; }

        public IUpdatesApiService Updates { get; set; }

        private TDcOption[] _dcOptions;

        private async Task<Tuple<AuthKey, int>> DoAuthentication()
        {
            Log.Info("Try do authentication");

            var reqPq = Step1ClientHelper.GetRequest(out var nonce);
            var step1 = Serializer.SerializeObject(reqPq);
            var step1Result = await MtProtoPlainSender.SendAndReceive(step1);
            var step1Response = Serializer.DeserializeObject(step1Result).Cast<TResPQ>();

            Log.Debug("First step is done");

            var reqDhParams = Step2ClientHelper.GetRequest(step1Response, ClientSettings.ServerPublicKey, out var newNonse);
            var step2 = Serializer.SerializeObject(reqDhParams);
            var step2Result = await MtProtoPlainSender.SendAndReceive(step2);
            var step2Response = Serializer.DeserializeObject(step2Result).As<TServerDHParamsOk>();

            Log.Debug("Second step is done");

            if (step2Response != null)
            {
                var request = Step3ClientHelper.GetRequest(step2Response, newNonse, out var clientAgree, out var serverTime);
                var step3 = Serializer.SerializeObject(request);
                var step3Result = await MtProtoPlainSender.SendAndReceive(step3);
                var step3Response = Serializer.DeserializeObject(step2Result).As<TServerDHParamsOk>();

                Log.Debug("Third step is done");

                return Tuple.Create(new AuthKey(clientAgree), serverTime);
            }

            throw new NotSupportedException();
        }

        public async Task ConnectAsync(bool reconnect = false)
        {
            if (ClientSettings.Session.AuthKey == null || reconnect)
            {
                var result = await DoAuthentication();
                ClientSettings.Session.AuthKey = result.Item1;
                ClientSettings.Session.TimeOffset = result.Item2;

                SessionStore.Save();
            }

            ConfirmationSendService.StartSendingConfirmation();
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

            var response = (TConfig)await SendRequestAsync(request);
            _dcOptions = response.DcOptions.Items.Cast<TDcOption>().ToArray();
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

        public async Task<TResult> SendRequestAsync<TResult>(IRequest<TResult> methodToExecute)
        {
            Log.Debug($"Send message of the constructor {methodToExecute}");

            BinaryReader resultReader;
            try
            {
                resultReader = await SendAndRecieve(methodToExecute);
            }
            catch (BadServerSaltException)
            {
                resultReader = await SendAndRecieve(methodToExecute);
            }

            return (TResult)Serializer.Deserialize(resultReader, typeof(TResult).GetTypeInfo());
        }

        private async Task<BinaryReader> SendAndRecieve(IRequest methodToExecute)
        {
            var sendTask = await Sender.Send(methodToExecute);
            var recieveTask = ResponseResultGetter.Recieve(sendTask.Item2);

            await sendTask.Item1;
            await recieveTask;

            return recieveTask.Result;
        }

        public async Task<IUpdates> SendMessageAsync(IInputPeer peer, string message)
        {
            if (!this.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            return await SendRequestAsync(
                       new RequestSendMessage
                       {
                           Peer = peer,
                           Message = message,
                           RandomId = TlHelpers.GenerateRandomLong()
                       });
        }
    }
}