namespace TelegramClient.Core.Exceptions
{
    using System;

    public class InvalidPhoneCodeException : Exception
    {
        internal InvalidPhoneCodeException(string msg) : base(msg)
        {
        }
    }
}