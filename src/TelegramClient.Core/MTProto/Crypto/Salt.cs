using System;
using System.Collections.Generic;

namespace TelegramClient.Core.MTProto.Crypto
{
    public class Salt : IComparable<Salt>
    {
        public Salt(int validSince, int validUntil, ulong salt)
        {
            ValidSince = validSince;
            ValidUntil = validUntil;
            Value = salt;
        }

        public int ValidSince { get; }

        public int ValidUntil { get; }

        public ulong Value { get; }

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
        public GetFutureSaltsResponse(ulong requestId, int now)
        {
            RequestId = requestId;
            Now = now;
        }

        public ulong RequestId { get; }

        public int Now { get; }

        public SaltCollection Salts { get; }

        public void AddSalt(Salt salt)
        {
            Salts.Add(salt);
        }
    }
}