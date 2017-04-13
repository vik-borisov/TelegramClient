using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TelegramClient.Core.Auth;
using TelegramClient.Core.Network;
using TelegramClient.Core.Utils;
using TelegramClient.Entities;
using TelegramClient.Entities.TL;
using TelegramClient.Entities.TL.Account;
using TelegramClient.Entities.TL.Auth;
using TelegramClient.Entities.TL.Contacts;
using TelegramClient.Entities.TL.Help;
using TelegramClient.Entities.TL.Messages;
using TelegramClient.Entities.TL.Upload;
using TlAuthorization = TelegramClient.Entities.TL.Auth.TlAuthorization;
using TlRequestSearch = TelegramClient.Entities.TL.Messages.TlRequestSearch;

namespace TelegramClient.Core
{
    public class Client : IDisposable
    {
        private readonly string _apiHash;
        private readonly int _apiId;
        private MtProtoSender _sender;
        private readonly Session _session;
        private TcpTransport _transport;
        private List<TlDcOption> _dcOptions;

        public Client(int apiId, string apiHash,
            ISessionStore store = null, string sessionUserId = "session")
        {
            if (apiId == default(int))
                throw new MissingApiConfigurationException("API_ID");
            if (string.IsNullOrEmpty(apiHash))
                throw new MissingApiConfigurationException("API_HASH");

            if (store == null)
                store = new FileSessionStore();

            TlContext.Init();
            _apiHash = apiHash;
            _apiId = apiId;

            _session = Session.TryLoadOrCreateNew(store, sessionUserId);
            _transport = new TcpTransport(_session.ServerAddress, _session.Port);
        }

        public void Dispose()
        {
            if (_transport != null)
            {
                _transport.Dispose();
                _transport = null;
            }
        }

        public async Task<bool> ConnectAsync(bool reconnect = false)
        {
            if (_session.AuthKey == null || reconnect)
            {
                var result = await Authenticator.DoAuthentication(_transport);
                _session.AuthKey = result.AuthKey;
                _session.TimeOffset = result.TimeOffset;
            }

            _sender = new MtProtoSender(_transport, _session);

            //set-up layer
            var config = new TlRequestGetConfig();
            var request = new TlRequestInitConnection
            {
                ApiId = _apiId,
                AppVersion = "1.0.0",
                DeviceModel = "PC",
                LangCode = "en",
                Query = config,
                SystemVersion = "Win 10.0"
            };
            var invokewithLayer = new TlRequestInvokeWithLayer {Layer = 57, Query = request};
            await _sender.Send(invokewithLayer);
            await _sender.Receive(invokewithLayer);

            _dcOptions = ((TlConfig) invokewithLayer.Response).DcOptions.Lists;

            return true;
        }

        private async Task ReconnectToDcAsync(int dcId)
        {
            if (_dcOptions == null || !_dcOptions.Any())
                throw new InvalidOperationException($"Can't reconnect. Establish initial connection first.");

            var dc = _dcOptions.First(d => d.Id == dcId);

            _transport = new TcpTransport(dc.IpAddress, dc.Port);
            _session.ServerAddress = dc.IpAddress;
            _session.Port = dc.Port;

            await ConnectAsync(true);
        }

        public bool IsUserAuthorized()
        {
            return _session.TlUser != null;
        }

        public async Task<bool> IsPhoneRegisteredAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (_sender == null)
                throw new InvalidOperationException("Not connected!");

            var authCheckPhoneRequest = new TlRequestCheckPhone {PhoneNumber = phoneNumber};
            var completed = false;
            while (!completed)
                try
                {
                    await _sender.Send(authCheckPhoneRequest);
                    await _sender.Receive(authCheckPhoneRequest);
                    completed = true;
                }
                catch (PhoneMigrationException e)
                {
                    await ReconnectToDcAsync(e.Dc);
                }
            return authCheckPhoneRequest.Response.PhoneRegistered;
        }

        public async Task<string> SendCodeRequestAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            var completed = false;

            TlRequestSendCode request = null;

            while (!completed)
            {
                request = new TlRequestSendCode {PhoneNumber = phoneNumber, ApiId = _apiId, ApiHash = _apiHash};
                try
                {
                    await _sender.Send(request);
                    await _sender.Receive(request);

                    completed = true;
                }
                catch (PhoneMigrationException ex)
                {
                    await ReconnectToDcAsync(ex.Dc);
                }
            }

            return request.Response.PhoneCodeHash;
        }

        public async Task<TlUser> MakeAuthAsync(string phoneNumber, string phoneCodeHash, string code)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(phoneCodeHash))
                throw new ArgumentNullException(nameof(phoneCodeHash));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            var request = new TlRequestSignIn
            {
                PhoneNumber = phoneNumber,
                PhoneCodeHash = phoneCodeHash,
                PhoneCode = code
            };
            await _sender.Send(request);
            await _sender.Receive(request);

            OnUserAuthenticated((TlUser) request.Response.User);

            return (TlUser) request.Response.User;
        }

        public async Task<TlPassword> GetPasswordSetting()
        {
            var request = new TlRequestGetPassword();

            await _sender.Send(request);
            await _sender.Receive(request);

            return (TlPassword) request.Response;
        }

        public async Task<TlUser> MakeAuthWithPasswordAsync(TlPassword password, string passwordStr)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(passwordStr);
            var rv = password.CurrentSalt.Concat(passwordBytes).Concat(password.CurrentSalt);

            byte[] passwordHash;
            using (var sha = SHA256.Create())
            {
                passwordHash = sha.ComputeHash(rv.ToArray());
            }

            var request = new TlRequestCheckPassword {PasswordHash = passwordHash};
            await _sender.Send(request);
            await _sender.Receive(request);

            OnUserAuthenticated((TlUser) request.Response.User);

            return (TlUser) request.Response.User;
        }

        public async Task<TlUser> SignUpAsync(string phoneNumber, string phoneCodeHash, string code, string firstName,
            string lastName)
        {
            var request = new TlRequestSignUp
            {
                PhoneNumber = phoneNumber,
                PhoneCode = code,
                PhoneCodeHash = phoneCodeHash,
                FirstName = firstName,
                LastName = lastName
            };
            await _sender.Send(request);
            await _sender.Receive(request);

            OnUserAuthenticated((TlUser) request.Response.User);

            return (TlUser) request.Response.User;
        }

        public async Task<T> SendRequestAsync<T>(TlMethod methodToExecute)
        {
            await _sender.Send(methodToExecute);
            await _sender.Receive(methodToExecute);

            var result = methodToExecute.GetType().GetProperty("Response").GetValue(methodToExecute);

            return (T) result;
        }

        public async Task<TlContacts> GetContactsAsync()
        {
            if (!IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var req = new TlRequestGetContacts {Hash = ""};

            return await SendRequestAsync<TlContacts>(req);
        }

        public async Task<TlAbsUpdates> SendMessageAsync(TlAbsInputPeer peer, string message)
        {
            if (!IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            return await SendRequestAsync<TlAbsUpdates>(
                new TlRequestSendMessage
                {
                    Peer = peer,
                    Message = message,
                    RandomId = Helpers.GenerateRandomLong()
                });
        }

        public async Task<bool> SendTypingAsync(TlAbsInputPeer peer)
        {
            var req = new TlRequestSetTyping
            {
                Action = new TlSendMessageTypingAction(),
                Peer = peer
            };
            return await SendRequestAsync<bool>(req);
        }

        public async Task<TlAbsDialogs> GetUserDialogsAsync()
        {
            var peer = new TlInputPeerSelf();
            return await SendRequestAsync<TlAbsDialogs>(
                new TlRequestGetDialogs {OffsetDate = 0, OffsetPeer = peer, Limit = 100});
        }

        public async Task<TlAbsUpdates> SendUploadedPhoto(TlAbsInputPeer peer, TlAbsInputFile file, string caption)
        {
            return await SendRequestAsync<TlAbsUpdates>(new TlRequestSendMedia
            {
                RandomId = Helpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TlInputMediaUploadedPhoto {File = file, Caption = caption},
                Peer = peer
            });
        }

        public async Task<TlAbsUpdates> SendUploadedDocument(
            TlAbsInputPeer peer, TlAbsInputFile file, string caption, string mimeType,
            TlVector<TlAbsDocumentAttribute> attributes)
        {
            return await SendRequestAsync<TlAbsUpdates>(new TlRequestSendMedia
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

        public async Task<TlFile> GetFile(TlAbsInputFileLocation location, int filePartSize, int offset = 0)
        {
            TlFile result = null;
            try
            {
                result = await SendRequestAsync<TlFile>(new TlRequestGetFile
                {
                    Location = location,
                    Limit = filePartSize,
                    Offset = offset
                });
            }
            catch (FileMigrationException ex)
            {
                var exportedAuth =
                    await SendRequestAsync<TlExportedAuthorization>(new TlRequestExportAuthorization {DcId = ex.Dc});

                var authKey = _session.AuthKey;
                var timeOffset = _session.TimeOffset;
                var serverAddress = _session.ServerAddress;
                var serverPort = _session.Port;

                await ReconnectToDcAsync(ex.Dc);
                var auth = await SendRequestAsync<TlAuthorization>(new TlRequestImportAuthorization
                {
                    Bytes = exportedAuth.Bytes,
                    Id = exportedAuth.Id
                });
                result = await GetFile(location, filePartSize, offset);

                _session.AuthKey = authKey;
                _session.TimeOffset = timeOffset;
                _transport = new TcpTransport(serverAddress, serverPort);
                _session.ServerAddress = serverAddress;
                _session.Port = serverPort;
                await ConnectAsync();
            }

            return result;
        }

        public async Task SendPingAsync()
        {
            await _sender.SendPingAsync();
        }

        public async Task<TlAbsMessages> GetHistoryAsync(TlAbsInputPeer peer, int offset, int maxId, int limit)
        {
            if (!IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var req = new TlRequestGetHistory
            {
                Peer = peer,
                AddOffset = offset,
                MaxId = maxId,
                Limit = limit
            };
            return await SendRequestAsync<TlAbsMessages>(req);
        }

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        public async Task<TlFound> SearchUserAsync(string q, int limit = 10)
        {
            var r = new TlRequestSearch
            {
                Q = q,
                Limit = limit
            };

            return await SendRequestAsync<TlFound>(r);
        }

        private void OnUserAuthenticated(TlUser tlUser)
        {
            _session.TlUser = tlUser;
            _session.SessionExpires = int.MaxValue;

            _session.Save();
        }
    }

    public class MissingApiConfigurationException : Exception
    {
        public const string InfoUrl = "https://github.com/sochix/TLSharp#quick-configuration";

        internal MissingApiConfigurationException(string invalidParamName) :
            base($"Your {invalidParamName} setting is missing. Adjust the configuration first, see {InfoUrl}")
        {
        }
    }

    public class InvalidPhoneCodeException : Exception
    {
        internal InvalidPhoneCodeException(string msg) : base(msg)
        {
        }
    }

    public class CloudPasswordNeededException : Exception
    {
        internal CloudPasswordNeededException(string msg) : base(msg)
        {
        }
    }
}