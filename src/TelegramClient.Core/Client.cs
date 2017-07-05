using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TelegramClient.Core.Auth;
using TelegramClient.Core.Network;
using TelegramClient.Core.Utils;

namespace TelegramClient.Core
{
    using System.IO;

    using log4net;
    using Autofac;

    using OpenTl.Schema;
    using OpenTl.Schema.Help;
    using OpenTl.Schema.Messages;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.ApiServies;
    using TelegramClient.Core.IoC;
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

        private async Task<Step3Response> DoAuthentication()
        {
            Log.Info("Try do authentication");

            var step1 = new Step1PqRequest();
            var step1Result = await MtProtoPlainSender.SendAndReceive(step1.ToBytes());
            var step1Response = step1.FromBytes(step1Result);

            Log.Debug("First step is done");

            var step2 = new Step2DhExchange();
            var step2Result = await MtProtoPlainSender.SendAndReceive(step2.ToBytes(
                                  step1Response.Nonce,
                                  step1Response.ServerNonce,
                                  step1Response.Fingerprints,
                                  step1Response.Pq));
            var step2Response = step2.FromBytes(step2Result);

            Log.Debug("Second step is done");

            var step3 = new Step3CompleteDhExchange();
            var step3Result = await MtProtoPlainSender.SendAndReceive(step3.ToBytes(
                step2Response.Nonce,
                step2Response.ServerNonce,
                step2Response.NewNonce,
                step2Response.EncryptedAnswer));
            var step3Response = step3.FromBytes(step3Result);

            Log.Debug("Third step is done");

            return step3Response;
        }

        public async Task ConnectAsync(bool reconnect = false)
        {
            if (ClientSettings.Session.AuthKey == null || reconnect)
            {
                var result = await DoAuthentication();
                ClientSettings.Session.AuthKey = result.AuthKey;
                ClientSettings.Session.TimeOffset = result.TimeOffset;

                SessionStore.Save();
            }

            //set-up layer
            var request = new RequestInitConnection
            {
                ApiId = ClientSettings.AppId,
                AppVersion = "1.0.0",
                DeviceModel = "PC",
                LangCode = "en",
                Query = new RequestGetConfig(),
                SystemVersion = "Win 10.0"
            };

            ConfirmationSendService.StartSendingConfirmation();
            ProtoRecieveService.StartReceiving();

            var response = (TConfig) await SendRequestAsync(new RequestInvokeWithLayer {Layer = 66, Query = request});
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

            return (TResult) Serializer.DeserializeObject(resultReader);
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