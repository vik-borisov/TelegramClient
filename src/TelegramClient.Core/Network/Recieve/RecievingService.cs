namespace TelegramClient.Core.Network.Recieve
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using TelegramClient.Core.Helpers;
    using TelegramClient.Core.MTProto.Crypto;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Network.Tcp;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;

    internal class RecievingService : IRecievingService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RecievingService));

        private CancellationTokenSource _recievingCts;

        public ITcpTransport TcpTransport { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public IConfirmationSendService ConfirmationSendService { get; set; }

        public Dictionary<uint, IRecieveHandler> RecieveHandlersMap { get; set; }

        public void StartReceiving()
        {
            if (_recievingCts != null && _recievingCts.IsCancellationRequested)
            {
                return;
            }

            _recievingCts = new CancellationTokenSource();

            Task.Run(
                () =>
                {
                    while (!_recievingCts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var recieveTask = TcpTransport.Receieve();
                            recieveTask.Wait(_recievingCts.Token);
                            var recieveData = recieveTask.Result;

                            var decodedData = DecodeMessage(recieveData);

                            Log.Debug($"Recieve message with remote id: {decodedData.Item2}");

                            ProcessReceivedMessage(decodedData.Item1);

                            ConfirmationSendService.AddForSend(decodedData.Item2);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Recieve message failed", e);
                        }
                    }
                },
                _recievingCts.Token);
        }

        public void StopRecieving()
        {
            _recievingCts?.Cancel();
        }

        private Tuple<byte[], ulong> DecodeMessage(byte[] body)
        {
            byte[] message;
            ulong remoteMessageId;
            int remoteSequence;

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
                    remoteMessageId = plaintextReader.ReadUInt64();
                    remoteSequence = plaintextReader.ReadInt32();
                    var msgLen = plaintextReader.ReadInt32();
                    message = plaintextReader.ReadBytes(msgLen);
                }
            }

            return Tuple.Create(message, remoteMessageId);
        }

        private void ProcessByRecieveHandler(BinaryReader reader, IRecieveHandler handler)
        {
            Log.Debug($"Handler found - {handler}");

            foreach (var processMessage in handler.HandleResponce(reader))
            {
                try
                {
                    ProcessReceivedMessage(processMessage);
                }
                catch (Exception ex)
                {
                    Log.Error("Cannot process message", ex);
                }
            }
        }

        private void ProcessContainerMessage(BinaryReader reader)
        {
            Log.Debug("Handle container");

            var size = reader.ReadInt32();

            for (var i = 0; i < size; i++)
            {
                var innerMessageId = reader.ReadUInt64();
                var innerSequence = reader.ReadInt32();
                var innerLength = reader.ReadInt32();

                Log.Debug($"Process responce with inner id = '{innerMessageId}' into container");

                ProcessReceivedMessage(reader.ReadBytes(innerLength));

                ConfirmationSendService.AddForSend(innerMessageId);
            }
        }

        private void ProcessReceivedMessage(byte[] message)
        {
            BinaryHelper.ReadBytes(
                message,
                reader =>
                {
                    var code = reader.ReadUInt32();

                    Log.Debug($"Try handle response with code = {code}");

                    switch (code)
                    {
                        case var c when RecieveHandlersMap.TryGetValue(c, out var handler):
                            ProcessByRecieveHandler(reader, handler);
                            break;
                        case 0x73f1f8dc:
                            ProcessContainerMessage(reader);
                            break;
                        default:
                            Log.Error($"Cannot handle response with code = {code}");
                            break;
                    }
                });
        }
    }
}