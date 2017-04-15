using System;
using System.Threading.Tasks;
using TelegramClient.Core.Utils;
using TelegramClient.Entities;
using TelegramClient.Entities.TL;
using TelegramClient.Entities.TL.Contacts;
using TelegramClient.Entities.TL.Messages;
using TlRequestSearch = TelegramClient.Entities.TL.Messages.TlRequestSearch;

namespace TelegramClient.Core
{
    public static class MessageExtentions
    {
        public static async Task<bool> SendTypingAsync(this ITelegramClient client, TlAbsInputPeer peer)
        {
            var req = new TlRequestSetTyping
            {
                Action = new TlSendMessageTypingAction(),
                Peer = peer
            };
            return await client.SendRequestAsync<bool>(req);
        }

        public static async Task<TlAbsDialogs> GetUserDialogsAsync(this ITelegramClient client, int limit = 100)
        {
            var getDialogs = new TlRequestGetDialogs
            {
                OffsetDate = 0,
                OffsetPeer = new TlInputPeerSelf(),
                Limit = limit
            };

            return await client.SendRequestAsync<TlAbsDialogs>(getDialogs);
        }

        public static async Task<TlAbsUpdates> SendUploadedPhoto(this ITelegramClient client, TlAbsInputPeer peer,
            TlAbsInputFile file, string caption)
        {
            return await client.SendRequestAsync<TlAbsUpdates>(new TlRequestSendMedia
            {
                RandomId = Helpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TlInputMediaUploadedPhoto {File = file, Caption = caption},
                Peer = peer
            });
        }

        public static async Task<TlAbsUpdates> SendUploadedDocument(
            this ITelegramClient client,
            TlAbsInputPeer peer,
            TlAbsInputFile file,
            string caption,
            string mimeType,
            TlVector<TlAbsDocumentAttribute> attributes)
        {
            return await client.SendRequestAsync<TlAbsUpdates>(new TlRequestSendMedia
            {
                RandomId = Helpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TlInputMediaUploadedDocument
                {
                    File = file,
                    Caption = caption,
                    MimeType = mimeType,
                    Attributes = attributes
                },
                Peer = peer
            });
        }

        public static async Task<TlAbsMessages> GetHistoryAsync(this ITelegramClient client, TlAbsInputPeer peer, int offset, int maxId, int limit)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var req = new TlRequestGetHistory
            {
                Peer = peer,
                AddOffset = offset,
                MaxId = maxId,
                Limit = limit
            };
            return await client.SendRequestAsync<TlAbsMessages>(req);
        }

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        public static async Task<TlFound> SearchUserAsync(this ITelegramClient client, string q, int limit = 10)
        {
            var r = new TlRequestSearch
            {
                Q = q,
                Limit = limit
            };

            return await client.SendRequestAsync<TlFound>(r);
        }
    }
}