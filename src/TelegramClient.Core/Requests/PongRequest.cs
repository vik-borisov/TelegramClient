using System;
using System.IO;
using TelegramClient.Entities;

namespace TelegramClient.Core.Requests
{
    public class PongRequest : TlMethod
    {
        public override int Constructor => 0x347773c5;

        private long _requestMessageID;

        public PongRequest(long requestMessageID)
        {
            _requestMessageID = requestMessageID;
        }

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(Constructor);
            writer.Write(_requestMessageID);
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