using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TelegramClient.Core.Network;
using TelegramClient.Entities.TL;
using TelegramClient.Entities.TL.Account;
using TelegramClient.Entities.TL.Auth;

namespace TelegramClient.Core
{
    using Autofac;

    using BarsGroup.CodeGuard;

    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    using TlAuthorization = TelegramClient.Entities.TL.Auth.TlAuthorization;

    public static class AuthExtentions
    {
        public static async Task<bool> IsPhoneRegisteredAsync(this ITelegramClient client, string phoneNumber)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var authCheckPhoneRequest = new TlRequestCheckPhone { PhoneNumber = phoneNumber };
            var completed = false;
            while (!completed)
            {    try
                {
                    await client.SendRequestAsync<TlRequestCheckPhone>(authCheckPhoneRequest);
                    completed = true;
                }
                catch (PhoneMigrationException e)
                {
                    await client.ReconnectToDcAsync(e.Dc);
                }
            }

            return authCheckPhoneRequest.Response.PhoneRegistered;
        }

        public static async Task<string> SendCodeRequestAsync(this ITelegramClient client, string phoneNumber)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var completed = false;
            var clientSettings = client.GetSettings();

            var request = new TlRequestSendCode { PhoneNumber = phoneNumber, ApiId = clientSettings.AppId, ApiHash = clientSettings.AppHash };
            while (!completed)
            {
                try
                {
                    await client.SendRequestAsync<TlSentCode>(request);

                    completed = true;
                }
                catch (PhoneMigrationException ex)
                {
                    await client.ReconnectToDcAsync(ex.Dc);
                }
            }

            return request.Response.PhoneCodeHash;
        }

        public static async Task<TlUser> MakeAuthAsync(this ITelegramClient client, string phoneNumber, string phoneCodeHash, string code)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();
            Guard.That(phoneCodeHash, nameof(phoneCodeHash)).IsNotNullOrWhiteSpace();
            Guard.That(code, nameof(code)).IsNotNullOrWhiteSpace();

            var request = new TlRequestSignIn
            {
                PhoneNumber = phoneNumber,
                PhoneCodeHash = phoneCodeHash,
                PhoneCode = code
            };

            await client.SendRequestAsync<TlAuthorization>(request);

            OnUserAuthenticated(client.Container, (TlUser)request.Response.User);

            return (TlUser)request.Response.User;
        }

        public static async Task<TlPassword> GetPasswordSetting(this ITelegramClient client)
        {
            var request = new TlRequestGetPassword();

            await client.SendRequestAsync<TlPassword>(request);

            return (TlPassword)request.Response;
        }

        public static async Task<TlUser> MakeAuthWithPasswordAsync(this ITelegramClient client, TlPassword password, string passwordStr)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(passwordStr);
            var rv = password.CurrentSalt.Concat(passwordBytes).Concat(password.CurrentSalt);

            byte[] passwordHash;
            using (var sha = SHA256.Create())
            {
                passwordHash = sha.ComputeHash(rv.ToArray());
            }

            var request = new TlRequestCheckPassword { PasswordHash = passwordHash };
            await client.SendRequestAsync<TlAuthorization>(request);

            OnUserAuthenticated(client.Container, (TlUser)request.Response.User);

            return (TlUser)request.Response.User;
        }

        public static async Task<TlUser> SignUpAsync(this ITelegramClient client, string phoneNumber, string phoneCodeHash, string code, string firstName,
            string lastName)
        {
            Guard.That(client.Container).IsNotNull();

            var request = new TlRequestSignUp
            {
                PhoneNumber = phoneNumber,
                PhoneCode = code,
                PhoneCodeHash = phoneCodeHash,
                FirstName = firstName,
                LastName = lastName
            };
            await client.SendRequestAsync<TlAuthorization>(request);

            OnUserAuthenticated(client.Container, (TlUser)request.Response.User);

            return (TlUser)request.Response.User;
        }

        private static void OnUserAuthenticated(IComponentContext container, TlUser tlUser)
        {
            var clientSettings = container.Resolve<IClientSettings>();
            Guard.That(clientSettings).IsNotNull();

            var session = clientSettings.Session;
            Guard.That(session).IsNotNull();

            session.TlUser = tlUser;
            session.SessionExpires = int.MaxValue;

            var sessionStore = container.Resolve<ISessionStore>();

            sessionStore.Save(session);
        }

        public static bool IsUserAuthorized(this ITelegramClient client)
        {
            Guard.That(client.Container).IsNotNull();

            var clientSettings = client.Container.Resolve<IClientSettings>();
            Guard.That(clientSettings).IsNotNull();
            Guard.That(clientSettings.Session).IsNotNull();

            return clientSettings.Session.TlUser != null;
        }
    }
}