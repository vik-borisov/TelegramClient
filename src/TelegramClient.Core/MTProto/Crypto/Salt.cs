namespace TelegramClient.Core.MTProto.Crypto
{
    using System;
    using System.Collections.Generic;

    public class Salt : IComparable<Salt>
    {
        public int ValidSince { get; }

        public int ValidUntil { get; }

        public ulong Value { get; }

        public Salt(int validSince, int validUntil, ulong salt)
        {
            ValidSince = validSince;
            ValidUntil = validUntil;
            Value = salt;
        }

        public int CompareTo(Salt other)
        {
            return ValidUntil.CompareTo(other.ValidSince);
        }
    }

    public class SaltCollection
    {
        private SortedSet<Salt> _salts;

        public int Count => _salts.Count;

        public void Add(Salt salt)
        {
            _salts.Add(salt);
        }

        // TODO: get actual salt and other...
    }

    public class GetFutureSaltsResponse
    {
        public ulong RequestId { get; }

        public int Now { get; }

        public SaltCollection Salts { get; }

        public GetFutureSaltsResponse(ulong requestId, int now)
        {
            RequestId = requestId;
            Now = now;
        }

        public void AddSalt(Salt salt)
        {
            Salts.Add(salt);
        }
    }
}