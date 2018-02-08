namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using Newtonsoft.Json;

    using OpenTl.Schema;
    using OpenTl.Schema.Help;
    using OpenTl.Schema.Serialization;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Network.Tcp;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;

    [SingleInstance(typeof(IRecievingService))]
    internal class RecievingService : IRecievingService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RecievingService));

        private CancellationTokenSource _recievingTokenSource;

        public ITcpTransport TcpTransport { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public IConfirmationSendService ConfirmationSendService { get; set; }

        public IDictionary<Type, IRecieveHandler> RecieveHandlersMap { get; set; }

        public IGZipPackedHandler ZipPackedHandler { get; set; }

        public IMtProtoSender Sender { get; set; }

        public IResponseResultGetter ResponseResultGetter { get; set; }

        public void Dispose()
        {
            _recievingTokenSource?.Cancel();
            TcpTransport?.Dispose();
        }

        public void StartReceiving()
        {
            if (_recievingTokenSource == null)
            {
                _recievingTokenSource = new CancellationTokenSource();
                StartRecievingTask(_recievingTokenSource.Token);
            }
        }

        private Tuple<byte[], long> DecodeMessage(byte[] body)
        {
            byte[] message;
            long remoteMessageId;

            using (var inputStream = new MemoryStream(body))
            using (var inputReader = new BinaryReader(inputStream))
            {
                if (inputReader.BaseStream.Length < 8)
                {
                    throw new InvalidOperationException("Can\'t decode packet");
                }

                var remoteAuthKeyId = inputReader.ReadUInt64(); // TODO: check auth key id
                var msgKey = inputReader.ReadBytes(16); // TODO: check msg_key correctness
                var keyData = TlHelpers.CalcKey(ClientSettings.Session.AuthKey.Data, msgKey, false);

                var plaintext = AES.DecryptAes(
                    keyData,
                    inputReader.ReadBytes((int)(inputStream.Length - inputStream.Position)));

                using (var plaintextStream = new MemoryStream(plaintext))
                using (var plaintextReader = new BinaryReader(plaintextStream))
                {
                    var remoteSalt = plaintextReader.ReadUInt64();
                    var remoteSessionId = plaintextReader.ReadUInt64();
                    remoteMessageId = plaintextReader.ReadInt64();
                    plaintextReader.ReadInt32();
                    var msgLen = plaintextReader.ReadInt32();
                    message = plaintextReader.ReadBytes(msgLen);
                }
            }

            return Tuple.Create(message, remoteMessageId);
        }

        private void ProcessReceivedMessage(byte[] message)
        {
            var obj = Serializer.DeserializeObject(message);

            ProcessReceivedMessage(obj);
        }

        private void ProcessReceivedMessage(IObject obj)
        {
            if (Log.IsDebugEnabled)
            {
                var jObject = JsonConvert.SerializeObject(obj);
                Log.Debug($"Try handle response for object: {obj} \n{jObject}");
            }

            switch (obj)
            {
                case var o when RecieveHandlersMap.TryGetValue(o.GetType(), out var handler):
                    Log.Debug($"Handler found - {handler}");
                    handler.HandleResponce(obj);
                    break;
                case TMsgContainer container:
                    foreach (var containerMessage in container.Messages)
                    {
                        ProcessReceivedMessage(containerMessage.Body);
                        ConfirmationSendService.AddForSend(containerMessage.MsgId);
                    }

                    break;
                case TgZipPacked zipPacked:
                    var unzippedData = ZipPackedHandler.HandleGZipPacked(zipPacked);
                    ProcessReceivedMessage(unzippedData);
                    break;
                default:
                    if (Log.IsErrorEnabled)
                    {
                        var jObject = JsonConvert.SerializeObject(obj);
                        Log.Error($"Cannot handle object: {obj} \n{jObject}");
                    }

                    break;
            }
        }

        private async Task StartRecievingTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var recieveData = await TcpTransport.Receieve().ConfigureAwait(false);

                    var decodedData = DecodeMessage(recieveData);

                    Log.Debug($"Receive message with remote id: {decodedData.Item2}");

                    ProcessReceivedMessage(decodedData.Item1);

                    ConfirmationSendService.AddForSend(decodedData.Item2);
                }
                catch (Exception e)
                {
                    Log.Error("Receive message failed. Reconnecting", e);

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
                                                  Query = new RequestGetConfig(),
                                                  SystemLangCode = "en",
                                                  SystemVersion = "Win 10.0"
                                              }
                                  };

                    try
                    {
                        await TcpTransport.Disconnect().ConfigureAwait(false);
                        
                        var sendTask = await Sender.SendWithConfim(request).ConfigureAwait(false);
                        
                        ResponseResultGetter.Receive(sendTask.Item2).ContinueWith(
                            async task =>
                            {
                                await sendTask.Item1.ConfigureAwait(false);
                            });
                    }
                    catch
                    {
                        Log.Error("Failed to reconnect", e);
                    }
                }
            }
        }
    }
}