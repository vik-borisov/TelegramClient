namespace TelegramClient.Core.Exceptions
{
    using System;

    public class CloudPasswordNeededException : Exception
    {
        internal CloudPasswordNeededException(string msg) : base(msg)
        {
        }
    }
}