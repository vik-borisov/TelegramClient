namespace TelegramClient.Core.Network.Exceptions
{
    using System;

    public class FloodException : Exception
    {
        internal FloodException(TimeSpan timeToWait)
            : base(
                $"Flood prevention. Telegram now requires your program to do requests again only after {timeToWait.TotalSeconds} seconds have passed ({nameof(TimeToWait)} property)." +
                " If you think the culprit of this problem may lie in TLSharp's implementation, open a Github issue please.")
        {
            TimeToWait = timeToWait;
        }

        public TimeSpan TimeToWait { get; }
    }
}