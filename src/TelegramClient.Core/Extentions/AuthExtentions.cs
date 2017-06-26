using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TelegramClient.Core
{
    using Autofac;

    using BarsGroup.CodeGuard;

    using OpenTl.Schema;
    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;

    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    using TAuthorization = OpenTl.Schema.Auth.TAuthorization;

    public static class AuthExtentions
    {
        public static async Task<ICheckedPhone> IsPhoneRegisteredAsync(this ITelegramClient client, string phoneNumber)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var authCheckPhoneRequest = new RequestCheckPhone
                                        {
                                            PhoneNumber = phoneNumber
                                        };
            while (true)
            {
                try
                {
                    return await client.SendRequestAsync(authCheckPhoneRequest);
                }
                catch (PhoneMigrationException e)
                {
                    await client.ReconnectToDcAsync(e.Dc);
                }
            }
        }

        public static async Task<ISentCode> SendCodeRequestAsync(this ITelegramClient client, string phoneNumber)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var clientSettings = client.GetSettings();

            var request = new RequestSendCode { PhoneNumber = phoneNumber, ApiId = clientSettings.AppId, ApiHash = clientSettings.AppHash };
            while (true)
            {
                try
                {
                    return await client.SendRequestAsync(request);
                }
                catch (PhoneMigrationException ex)
                {
                    await client.ReconnectToDcAsync(ex.Dc);
                }
            }
        }

        public static async Task<TUser> MakeAuthAsync(this ITelegramClient client, string phoneNumber, string phoneCodeHash, string code)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();
            Guard.That(phoneCodeHash, nameof(phoneCodeHash)).IsNotNullOrWhiteSpace();
            Guard.That(code, nameof(code)).IsNotNullOrWhiteSpace();

            var request = new RequestSignIn
            {
                PhoneNumber = phoneNumber,
                PhoneCodeHash = phoneCodeHash,
                PhoneCode = code
            };

            var result = (TAuthorization) await client.SendRequestAsync(request);

            OnUserAuthenticated(client.Container, (TUser)result.User);

            return (TUser)result.User;
        }

        public static async Task<IPassword> GetPasswordSetting(this ITelegramClient client)
        {
            return await client.SendRequestAsync(new RequestGetPassword());
        }

        public static async Task<TUser> MakeAuthWithPasswordAsync(this ITelegramClient client, TPassword password, string passwordStr)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(passwordStr);
            var rv = password.CurrentSalt.Concat(passwordBytes).Concat(password.CurrentSalt);

            byte[] passwordHash;
            using (var sha = SHA256.Create())
            {
                passwordHash = sha.ComputeHash(rv.ToArray());
            }

            var request = new RequestCheckPassword { PasswordHash = passwordHash };
            var result = (TAuthorization) await client.SendRequestAsync(request);

            OnUserAuthenticated(client.Container, (TUser)result.User);

            return (TUser)result.User;
        }

        public static async Task<TUser> SignUpAsync(this ITelegramClient client, string phoneNumber, string phoneCodeHash, string code, string firstName,
            string lastName)
        {
            Guard.That(client.Container).IsNotNull();

            var request = new RequestSignUp
            {
                PhoneNumber = phoneNumber,
                PhoneCode = code,
                PhoneCodeHash = phoneCodeHash,
                FirstName = firstName,
                LastName = lastName
            };
           var result =  (TAuthorization) await client.SendRequestAsync(request) ;

            OnUserAuthenticated(client.Container, (TUser)result.User);

            return (TUser)result.User;
        }

        private static void OnUserAuthenticated(IComponentContext container, TUser tlUser)
        {
            var clientSettings = container.Resolve<IClientSettings>();
            Guard.That(clientSettings).IsNotNull();

            var session = clientSettings.Session;
            Guard.That(session).IsNotNull();

            session.User = tlUser;
            session.SessionExpires = int.MaxValue;

            var sessionStore = container.Resolve<ISessionStore>();

            sessionStore.Save();
        }

        public static bool IsUserAuthorized(this ITelegramClient client)
        {
            Guard.That(client.Container).IsNotNull();

            var clientSettings = client.Container.Resolve<IClientSettings>();
            Guard.That(clientSettings).IsNotNull();
            Guard.That(clientSettings.Session).IsNotNull();

            return clientSettings.Session.User != null;
        }
    }
}