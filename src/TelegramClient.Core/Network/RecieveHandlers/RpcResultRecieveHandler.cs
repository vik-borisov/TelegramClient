namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;
    using System.Text.RegularExpressions;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class RpcResultRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RpcResultRecieveHandler));

        public Type[] HandleCodes { get; } = { typeof(TRpcResult) };

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IResponseResultSetter ResponseResultSetter { get; set; }

        public IGZipPackedHandler ZipPackedHandler { get; set; }

        public void HandleResponce(IObject obj)
        {
            Log.Debug("Handle RpcResult");

            var message = obj.Cast<TRpcResult>();

            Log.Debug($"Process RpcResult  with request id = '{message.ReqMsgId}'");

            ConfirmationRecieveService.ConfirmRequest(message.ReqMsgId);

            switch (message.Result)
            {
                case TRpcError error:
                    HandleRpcError(message.ReqMsgId, error);
                    break;

                case TgZipPacked zipPacked:
                    var result = ZipPackedHandler.HandleGZipPacked(zipPacked);
                    ResponseResultSetter.ReturnResult(message.ReqMsgId, result);
                    break;

                default:
                    ResponseResultSetter.ReturnResult(message.ReqMsgId, message.Result);
                    break;
            }
        }

        private void HandleRpcError(long messageReqMsgId, TRpcError error)
        {
            // rpc_error

            Log.Warn($"Recieve error from server: {error.ErrorMessage}");

            Exception exception;
            switch (error.ErrorMessage)
            {
                case var floodMessage when floodMessage.StartsWith("FLOOD_WAIT_"):
                    var floodMessageTime = Regex.Match(floodMessage, @"\d+").Value;
                    var seconds = int.Parse(floodMessageTime);
                    exception = new FloodException(TimeSpan.FromSeconds(seconds));
                    break;

                case var phoneMigrate when phoneMigrate.StartsWith("PHONE_MIGRATE_"):
                    var phoneMigrateDcNumber = Regex.Match(phoneMigrate, @"\d+").Value;
                    var phoneMigrateDcIdx = int.Parse(phoneMigrateDcNumber);
                    exception = new PhoneMigrationException(phoneMigrateDcIdx);
                    break;

                case var fileMigrate when fileMigrate.StartsWith("FILE_MIGRATE_"):
                    var fileMigrateDcNumber = Regex.Match(fileMigrate, @"\d+").Value;
                    var fileMigrateDcIdx = int.Parse(fileMigrateDcNumber);
                    exception = new FileMigrationException(fileMigrateDcIdx);
                    break;

                case var userMigrate when userMigrate.StartsWith("FILE_MIGRATE_"):
                    var userMigrateDcNumber = Regex.Match(userMigrate, @"\d+").Value;
                    var userMigrateDcIdx = int.Parse(userMigrateDcNumber);
                    exception = new UserMigrationException(userMigrateDcIdx);
                    break;

                case "PHONE_CODE_INVALID":
                    exception = new InvalidPhoneCodeException("The numeric code used to authenticate does not match the numeric code sent by SMS/Telegram");
                    break;

                case "SESSION_PASSWORD_NEEDED":
                    exception = new CloudPasswordNeededException("This Account has Cloud Password !");
                    break;

                default:
                    exception = new InvalidOperationException(error.ErrorMessage);
                    break;
            }

            ResponseResultSetter.ReturnException(messageReqMsgId, exception);
        }
    }
}