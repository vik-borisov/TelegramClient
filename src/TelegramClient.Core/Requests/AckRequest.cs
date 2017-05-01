﻿using System;
using System.Collections.Generic;
using System.IO;
using TelegramClient.Entities;

namespace TelegramClient.Core.Requests
{
    public class AckRequest : TlMethod
    {
        private readonly HashSet<ulong> _msgs;

        public AckRequest(HashSet<ulong> msgs)
        {
            _msgs = msgs;
        }

        public override bool Confirmed => false;

        public override int Constructor => 0x62d6b459;

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(0x62d6b459); // msgs_ack
            writer.Write(0x1cb5c415); // Vector
            writer.Write(_msgs.Count);
            foreach (var messageId in _msgs)
            {
                writer.Write(messageId);
            }
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