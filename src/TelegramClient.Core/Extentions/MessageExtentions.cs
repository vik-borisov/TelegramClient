using System;
using System.Threading.Tasks;
using TelegramClient.Core.Utils;

namespace TelegramClient.Core
{
    using OpenTl.Schema;
    using OpenTl.Schema.Contacts;
    using OpenTl.Schema.Messages;

    using RequestSearch = OpenTl.Schema.Contacts.RequestSearch;

    public static class MessageExtentions
    {
        public static async Task<bool> SendTypingAsync(this ITelegramClient client, IInputPeer peer)
        {
            var req = new RequestSetTyping
            {
                Action = new TSendMessageTypingAction(),
                Peer = peer
            };
            
            return await client.SendRequestAsync(req);
        }

        public static async Task<IDialogs> GetUserDialogsAsync(this ITelegramClient client, int limit = 100)
        {
            var getDialogs = new RequestGetDialogs
            {
                OffsetDate = 0,
                OffsetPeer = new TInputPeerSelf(),
                Limit = limit
            };

            return await client.SendRequestAsync(getDialogs);
        }

        public static async Task<IUpdates> SendUploadedPhoto(this ITelegramClient client, IInputPeer peer, IInputFile file, string caption)
        {
            return await client.SendRequestAsync(new RequestSendMedia
            {
                RandomId = TlHelpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TInputMediaUploadedPhoto {File = file, Caption = caption},
                Peer = peer
            });
        }

        public static async Task<IUpdates> SendUploadedDocument(
            this ITelegramClient client,
            IInputPeer peer,
            IInputFile file,
            string caption,
            string mimeType,
            TVector<IDocumentAttribute> attributes)
        {
            return await client.SendRequestAsync<IUpdates>(new RequestSendMedia
            {
                RandomId = TlHelpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TInputMediaUploadedDocument
                {
                    File = file,
                    Caption = caption,
                    MimeType = mimeType,
                    Attributes = attributes
                },
                Peer = peer
            });
        }

        public static async Task<IMessages> GetHistoryAsync(this ITelegramClient client, IInputPeer peer, int offset, int maxId, int limit)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var req = new RequestGetHistory
            {
                Peer = peer,
                AddOffset = offset,
                MaxId = maxId,
                Limit = limit
            };
            return await client.SendRequestAsync(req);
        }

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        public static async Task<IFound> SearchUserAsync(this ITelegramClient client, string q, int limit = 10)
        {
            var r = new RequestSearch
            {
                Q = q,
                Limit = limit
            };

            return await client.SendRequestAsync(r);
        }
    }
}