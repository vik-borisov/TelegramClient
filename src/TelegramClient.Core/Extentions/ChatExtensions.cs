using System;
using System.Threading.Tasks;

namespace TelegramClient.Core
{
    using OpenTl.Schema;
    using OpenTl.Schema.Messages;

    /// <summary>
    /// API methods for working with chats.
    /// </summary>
    public static class ChatExtensions
    {
        /// <summary>
        /// Returns chat basic info on their IDs.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="ids">Identifiers of chats</param>
        /// <returns>
        /// Object contains list of chats with auxiliary data.
        /// </returns>
        public static async Task<IChats> GetChatsAsync(this ITelegramClient client, TVector<int> ids)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestGetChats { Id = ids };

            return await client.SendRequestAsync(request);
        }

        /// <summary>
        /// Returns full chat info according to its ID.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="chatId">Chat's identifier</param>
        /// <returns>
        /// Object contains extended info on chat with auxiliary data.
        /// </returns>
        public static async Task<OpenTl.Schema.Messages.IChatFull> GetFullChatAsync(this ITelegramClient client, int chatId)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestGetFullChat { ChatId = chatId };

            return await client.SendRequestAsync(request);
        }

        /// <summary>
        /// Chanages chat name and sends a service message on it.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="title">New chat name, different from the old one</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during an action.
        /// </returns>
        public static async Task<IUpdates> EditChatTitle(this ITelegramClient client, int chatId, string title)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestEditChatTitle { ChatId = chatId, Title = title };

            return await client.SendRequestAsync(request);
        }

        /// <summary>
        /// Changes chat photo and sends a service message on it.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="photo">Photo to be set</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during an action.
        /// </returns>
        public static async Task<IUpdates> EditChatPhoto(this ITelegramClient client, int chatId, IInputChatPhoto photo)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestEditChatPhoto
            {
                ChatId = chatId,
                Photo = photo
            };

            return await client.SendRequestAsync(request);
        }

        /// <summary>
        /// Adds a user to a chat and sends a service message on it.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="user">User ID to be added</param>
        /// <param name="limit">Number of last messages to be forwarded</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object contains info on one message with auxiliary data and data on the current state of updates.
        /// </returns>
        public static async Task<IUpdates> AddChatUserAsync(this ITelegramClient client, int chatId, IInputUser user, int limit)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestAddChatUser
            {
                ChatId = chatId,
                UserId = user,
                FwdLimit = limit
            };

            return await client.SendRequestAsync(request);
        }

        /// <summary>
        /// Deletes a user from a chat and sends a service message on it.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="user">User ID to be deleted</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during the action.
        /// </returns>
        public static async Task<IUpdates> DeleteChatUser(this ITelegramClient client, int chatId, IInputUser user)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestDeleteChatUser
            {
                ChatId = chatId,
                UserId = user
            };

            return await client.SendRequestAsync(request);
        }

        /// <summary>
        /// Creates a new chat.
        /// </summary>
        /// <param name="client">Telegram client instance</param>
        /// <param name="title">Chat name</param>
        /// <param name="users">List of user IDs to be invited</param>
        /// <returns>
        /// Returns a <see cref="IUpdates"/> object containing a service message sent during an action.
        /// </returns>
        public static async Task<IUpdates> CreateChatAsync(this ITelegramClient client, string title, TVector<IInputUser> users)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var request = new RequestCreateChat
            {
                Title = title,
                Users = users
            };

            return await client.SendRequestAsync(request);
        }
    }
}
