namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System;

    using log4net;

    using OpenTl.Schema;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    public class MsgNewDetailedInfoRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MsgNewDetailedInfoRecieveHandler));

        public Type[] HandleCodes { get; } =  { typeof(TMsgNewDetailedInfo) };

        public void HandleResponce(IObject obj)
        {
            var info = obj.Cast<TMsgDetailedInfo>();
            
            Log.Debug("Handle a TMsgNewDetailedInfo");
        }
    }
}