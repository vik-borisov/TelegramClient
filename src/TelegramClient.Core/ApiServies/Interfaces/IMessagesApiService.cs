namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Messages;

    using IChatFull = OpenTl.Schema.Messages.IChatFull;

    public interface IMessagesApiService
    {
        /// <summary>Adds a user to a chat and sends a service message on it.</summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="user">User ID to be added</param>
        /// <param name="limit">Number of last messages to be forwarded</param>
        /// <returns>
        ///     Returns a <see cref="IUpdates" /> object contains info on one message with auxiliary data and data on the
        ///     current state of updates.
        /// </returns>
        Task<IUpdates> AddChatUserAsync(int chatId, IInputUser user, int limit, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Creates a new chat.</summary>
        /// <param name="title">Chat name</param>
        /// <param name="users">List of user IDs to be invited</param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during an action.</returns>
        Task<IUpdates> CreateChatAsync(string title, TVector<IInputUser> users, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Deletes a user from a chat and sends a service message on it.</summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="user">User ID to be deleted</param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during the action.</returns>
        Task<IUpdates> DeleteChatUser(int chatId, IInputUser user, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Deletes communication history.</summary>
        /// <param name="peer">User or chat, communication history of which will be deleted</param>
        /// <param name="maxId">
        ///     If a positive value was transferred, the method will return only messages with IDs less than the
        ///     set one
        /// </param>
        /// <param name="justClear">Delete as non-recoverable or just clear the history</param>
        /// <returns>Returns a <see cref="IAffectedHistory" /> object containing a affected history</returns>
        Task<IAffectedHistory> DeleteHistoryAsync(IInputPeer peer, int maxId, bool justClear, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Deletes messages by their IDs.</summary>
        /// <param name="ids">Identifiers of messages</param>
        /// <param name="revoke">Delete messages for everyone</param>
        /// <returns>Returns a <see cref="IAffectedMessages" /> object containing a affected messages</returns>
        Task<IAffectedMessages> DeleteMessagesAsync(TVector<int> ids, bool revoke, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Changes chat photo and sends a service message on it.</summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="photo">Photo to be set</param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during an action.</returns>
        Task<IUpdates> EditChatPhoto(int chatId, IInputChatPhoto photo, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Chanages chat name and sends a service message on it.</summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <param name="title">New chat name, different from the old one</param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during an action.</returns>
        Task<IUpdates> EditChatTitle(int chatId, string title, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Forwards single messages.</summary>
        /// <param name="peer">User or chat where a message will be forwarded</param>
        /// <param name="messageId">Forwarded message ID</param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during an action.</returns>
        Task<IUpdates> ForwardMessageAsync(IInputPeer peer, int messageId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Forwards messages by their IDs.</summary>
        /// <param name="fromPeer">User or chat from where a message will be forwarded</param>
        /// <param name="toPeer">User or chat where a message will be forwarded</param>
        /// <param name="ids">Forwarded message IDs</param>
        /// <param name="silent"></param>
        /// <param name="withMyScore"></param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during an action.</returns>
        Task<IUpdates> ForwardMessagesAsync(IInputPeer fromPeer, IInputPeer toPeer, TVector<int> ids, bool silent, bool withMyScore, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Returns chat basic info on their IDs.</summary>
        /// <param name="ids">Identifiers of chats</param>
        /// <returns>Object contains list of chats with auxiliary data.</returns>
        Task<IChats> GetChatsAsync(TVector<int> ids, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Returns full chat info according to its ID.</summary>
        /// <param name="chatId">Chat's identifier</param>
        /// <returns>Object contains extended info on chat with auxiliary data.</returns>
        Task<IChatFull> GetFullChatAsync(int chatId, CancellationToken cancellationToken = default(CancellationToken));

        Task<IMessages> GetHistoryAsync(IInputPeer peer, int offset, int maxId, int limit, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Returns the list of messages by their IDs.</summary>
        /// <param name="ids">Identifiers of messages</param>
        /// <returns>Object contains list of messages</returns>
        Task<IMessages> GetMessagesAsync(TVector<int> ids, CancellationToken cancellationToken = default(CancellationToken));

        Task<IDialogs> GetUserDialogsAsync(int limit = 100, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Marks message history as read.</summary>
        /// <param name="peer">User or group to receive the message</param>
        /// <param name="maxId">
        ///     If a positive value is passed, only messages with identifiers less or equal than the given one will
        ///     be read
        /// </param>
        /// <returns>Returns a <see cref="IAffectedMessages" /> object containing a affected messages</returns>
        Task<IAffectedMessages> ReadHistoryAsync(IInputPeer peer, int maxId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Notifies the sender about the recipient having listened a voice message or watched a video.</summary>
        /// <param name="ids">Identifiers of messages</param>
        /// <returns>Returns a <see cref="IAffectedMessages" /> object containing a affected messages</returns>
        Task<IAffectedMessages> ReadMessageContentsAsync(TVector<int> ids, CancellationToken cancellationToken = default(CancellationToken));

        Task<TVector<TReceivedNotifyMessage>> ReceivedMessagesAsync(int maxId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Sends a non-text message.</summary>
        /// <param name="peer">User or group to receive the message</param>
        /// <param name="media">Message contents</param>
        /// <returns>Returns a <see cref="IUpdates" /> object containing a service message sent during the action.</returns>
        Task<IUpdates> SendMediaAsync(IInputPeer peer, IInputMedia media, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>Send text message to peer</summary>
        /// <returns>Object contains list of updates.</returns>
        Task<IUpdates> SendMessageAsync(IInputPeer peer, string message, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SendTypingAsync(IInputPeer peer, CancellationToken cancellationToken = default(CancellationToken));

        Task<IUpdates> SendUploadedDocument(IInputPeer peer, IInputFile file, string caption, string mimeType, TVector<IDocumentAttribute> attributes, CancellationToken cancellationToken = default(CancellationToken));

        Task<IUpdates> SendUploadedPhoto(IInputPeer peer, IInputFile file, string caption, CancellationToken cancellationToken = default(CancellationToken));
    }
}