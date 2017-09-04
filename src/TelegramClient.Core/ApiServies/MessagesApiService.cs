namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Messages;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Utils;

    /// <summary>
    /// API methods for working with messages.
    /// </summary>
    [SingleInstance(typeof(IMessagesApiService))]
    internal class MessagesApiService : IMessagesApiService
    {
        public IAuthApiService AuthApiService { get; set; }

        public ISenderService SenderService { get; set; }

        /// <summary>
        /// Send text message to peer
        /// </summary>
        /// <returns>
        /// Object contains list of updates.
        /// </returns>
        public async Task<IUpdates> SendMessageAsync(IInputPeer peer, string message)
        {
            EnsureUserAuthorized();

            return await SenderService.SendRequestAsync(
                       new RequestSendMessage
                       {
                           Peer = peer,
                           Message = message,
                           RandomId = TlHelpers.GenerateRandomLong()
                       }).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns chat basic info on their IDs.
        /// </summary>
        /// <param name="ids">Identifiers of chats</param>
        /// <returns>
        /// Object contains list of chats with auxiliary data.
        /// </returns>
        public  async Task<IChats> GetChatsAsync(TVector<int> ids)
        {
            EnsureUserAuthorized();

            var request = new RequestGetChats { Id = ids };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns full chat info according to its ID.
        /// </summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <returns>
        /// Object contains extended info on chat with auxiliary data.
        /// </returns>
        public  async Task<OpenTl.Schema.Messages.IChatFull> GetFullChatAsync(int chatId)
        {
            EnsureUserAuthorized();

            var request = new RequestGetFullChat { ChatId = chatId };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Chanages chat name and sends a service message on it.
        /// </summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="title">New chat name, different from the old one</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during an action.
        /// </returns>
        public  async Task<IUpdates> EditChatTitle(int chatId, string title)
        {
            EnsureUserAuthorized();

            var request = new RequestEditChatTitle { ChatId = chatId, Title = title };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Changes chat photo and sends a service message on it.
        /// </summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="photo">Photo to be set</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during an action.
        /// </returns>
        public  async Task<IUpdates> EditChatPhoto(int chatId, IInputChatPhoto photo)
        {
            EnsureUserAuthorized();

            var request = new RequestEditChatPhoto
            {
                ChatId = chatId,
                Photo = photo
            };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }

       

        /// <summary>
        /// Adds a user to a chat and sends a service message on it.
        /// </summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="user">User ID to be added</param>
        /// <param name="limit">Number of last messages to be forwarded</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object contains info on one message with auxiliary data and data on the current state of updates.
        /// </returns>
        public  async Task<IUpdates> AddChatUserAsync(int chatId, IInputUser user, int limit)
        {
            EnsureUserAuthorized();

            var request = new RequestAddChatUser
            {
                ChatId = chatId,
                UserId = user,
                FwdLimit = limit
            };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a user from a chat and sends a service message on it.
        /// </summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="user">User ID to be deleted</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during the action.
        /// </returns>
        public  async Task<IUpdates> DeleteChatUser(int chatId, IInputUser user)
        {
            EnsureUserAuthorized();

            var request = new RequestDeleteChatUser
            {
                ChatId = chatId,
                UserId = user
            };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new chat.
        /// </summary>
        /// <param name="title">Chat name</param>
        /// <param name="users">List of user IDs to be invited</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during an action.
        /// </returns>
        public  async Task<IUpdates> CreateChatAsync(string title, TVector<IInputUser> users)
        {
            EnsureUserAuthorized();

            var request = new RequestCreateChat
            {
                Title = title,
                Users = users
            };

            return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
        }
        public  async Task<bool> SendTypingAsync( IInputPeer peer)
        {
            var req = new RequestSetTyping
            {
                Action = new TSendMessageTypingAction(),
                Peer = peer
            };

            return await SenderService.SendRequestAsync(req).ConfigureAwait(false);
        }

        public  async Task<IDialogs> GetUserDialogsAsync( int limit = 100)
        {
            EnsureUserAuthorized();

            var getDialogs = new RequestGetDialogs
            {
                OffsetDate = 0,
                OffsetPeer = new TInputPeerSelf(),
                Limit = limit
            };

            return await SenderService.SendRequestAsync(getDialogs).ConfigureAwait(false);
        }

        public  async Task<IUpdates> SendUploadedPhoto( IInputPeer peer, IInputFile file, string caption)
        {
            EnsureUserAuthorized();

            return await SenderService.SendRequestAsync(new RequestSendMedia
            {
                RandomId = TlHelpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TInputMediaUploadedPhoto { File = file, Caption = caption },
                Peer = peer
            }).ConfigureAwait(false);
        }

        public  async Task<IUpdates> SendUploadedDocument(
            
            IInputPeer peer,
            IInputFile file,
            string caption,
            string mimeType,
            TVector<IDocumentAttribute> attributes)
        {
            EnsureUserAuthorized();

            return await SenderService.SendRequestAsync(new RequestSendMedia
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
            }).ConfigureAwait(false);
        }

        public  async Task<IMessages> GetHistoryAsync( IInputPeer peer, int offset, int maxId, int limit)
        {
            EnsureUserAuthorized();

             var req = new RequestGetHistory
            {
                Peer = peer,
                AddOffset = offset,
                MaxId = maxId,
                Limit = limit
            };
            return await SenderService.SendRequestAsync(req).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the list of messages by their IDs.
        /// </summary>
        /// <param name="ids">Identifiers of messages</param>
        /// <returns>
        /// Object contains list of messages
        /// </returns>
        public  async Task<IMessages> GetMessagesAsync(TVector<int> ids)
        {
            EnsureUserAuthorized();

            var getMessagesRequest = new RequestGetMessages
                                     {
                                         Id = ids
                                     };

            return await SenderService.SendRequestAsync(getMessagesRequest);
        }

        /// <summary>
        /// Sends a non-text message.
        /// </summary>
        /// <param name="peer">User or group to receive the message</param>
        /// <param name="media">Message contents</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during the action.
        /// </returns>
        public  async Task<IUpdates> SendMediaAsync(IInputPeer peer, IInputMedia media)
        {
            EnsureUserAuthorized();

            var sendMedia = new RequestSendMedia()
                            {
                                RandomId = TlHelpers.GenerateRandomLong(),
                                Peer = peer,
                                Media = media,
                                Background = false,
                                ClearDraft = false
                            };

            return await SenderService.SendRequestAsync(sendMedia);
        }

        /// <summary>
        /// Marks message history as read.
        /// </summary>
        /// <param name="peer">User or group to receive the message</param>
        /// <param name="maxId">If a positive value is passed, only messages with identifiers less or equal than the given one will be read</param>
        /// <returns>Returns a <see cref="IAffectedMessages"/> object containing a affected messages</returns>
        public  async Task<IAffectedMessages> ReadHistoryAsync(IInputPeer peer, int maxId)
        {
            EnsureUserAuthorized();

            var readHistory = new RequestReadHistory()
                              {
                                  Peer = peer,
                                  MaxId = maxId
                              };

            return await SenderService.SendRequestAsync(readHistory);
        }

        /// <summary>
        /// Notifies the sender about the recipient having listened a voice message or watched a video.
        /// </summary>
        /// <param name="ids">Identifiers of messages</param>
        /// <returns>Returns a <see cref="IAffectedMessages"/> object containing a affected messages</returns>
        public  async Task<IAffectedMessages> ReadMessageContentsAsync(TVector<int> ids)
        {
            EnsureUserAuthorized();

            var readMessageContents = new RequestReadMessageContents()
                                      {
                                          Id = ids
                                      };

            return await SenderService.SendRequestAsync(readMessageContents);
        }

        private void EnsureUserAuthorized()
        {
            if (!AuthApiService.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");
        }
    }
}
