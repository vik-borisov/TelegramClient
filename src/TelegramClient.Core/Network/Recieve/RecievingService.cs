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

            Task.Run(() =>
                {
                    while (!_recievingCts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var recieveTask = TcpTransport.Receieve();
                            recieveTask.Wait(_recievingCts.Token);
                            var result = recieveTask.Result;
                            ProcessReceivedMessage(result);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Recieve message failed", e);
                        }
                    }
                }, _recievingCts.Token);
        }

        public void StopRecieving()
        {
            _recievingCts?.Cancel();
        }

        private void ProcessReceivedMessage(byte[] message)
        {
            var result = DecodeMessage(message);

            Log.Debug($"Recieve message with remote id: {result.Item2}");

            BinaryHelper.ReadBytes(result.Item1,
                reader =>
                {
                    var code = reader.ReadUInt32();

                    Log.Debug($"Try handle response with code = {code}");

                    if (RecieveHandlersMap.TryGetValue(code, out var handler))
                    {
                        Log.Debug($"Handler found - {handler}");

                        handler.HandleResponce(reader);
                    }
                    else
                    {
                        Log.Debug($"Cannot handle response with code = {code}");
                    }
                });

            ConfirmationSendService.AddForSend(result.Item2);
        }

        private Tuple<byte[], ulong, int> DecodeMessage(byte[] body)
        {
            byte[] message;
            ulong remoteMessageId;
            int remoteSequence;

            using (var inputStream = new MemoryStream(body))
            using (var inputReader = new BinaryReader(inputStream))
            {
                if (inputReader.BaseStream.Length < 8)
                    throw new InvalidOperationException("Can\'t decode packet");

                var remoteAuthKeyId = inputReader.ReadUInt64(); // TODO: check auth key id
                var msgKey = inputReader.ReadBytes(16); // TODO: check msg_key correctness
                var keyData = TlHelpers.CalcKey(ClientSettings.Session.AuthKey.Data, msgKey, false);

                var plaintext = AES.DecryptAes(keyData,
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

            return new Tuple<byte[], ulong, int>(message, remoteMessageId, remoteSequence);
        }
    }
}