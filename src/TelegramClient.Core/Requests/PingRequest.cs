using System;
using System.IO;
using TelegramClient.Core.Utils;
using TelegramClient.Entities;

namespace TelegramClient.Core.Requests
{
    public class PingRequest : TlMethod
    {
        public override int Constructor => 0x7abe77ec;

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(Constructor);
            writer.Write(Helpers.GenerateRandomLong());
        }

        public override void DeserializeBody(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeResponse(BinaryReader stream)
        {
            throw new NotImplementedException();
        }
    }
}