namespace TelegramClient.Core.ApiServies
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using BarsGroup.CodeGuard;

    using OpenTl.Schema;
    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    using TAuthorization = OpenTl.Schema.Auth.TAuthorization;

    [SingleInstance(typeof(IAuthApiService))]
    internal class AuthApiService : IAuthApiService
    {
        public ISenderService SenderService { get; set; }

        public IConnectApiService ConnectApiService { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public ISessionStore SessionStore { get; set; }

        public async Task<ICheckedPhone> IsPhoneRegisteredAsync(string phoneNumber)
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
                    return await SenderService.SendRequestAsync(authCheckPhoneRequest).ConfigureAwait(false);
                }
                catch (PhoneMigrationException e)
                {
                    await ConnectApiService.ReconnectToDcAsync(e.Dc).ConfigureAwait(false);
                }
            }
        }

        public async Task<ISentCode> SendCodeRequestAsync(string phoneNumber)
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var request = new RequestSendCode
                          {
                              PhoneNumber = phoneNumber,
                              ApiId = ClientSettings.AppId,
                              ApiHash = ClientSettings.AppHash
                          };
            while (true)
            {
                try
                {
                    return await SenderService.SendRequestAsync(request).ConfigureAwait(false);
                }
                catch (PhoneMigrationException ex)
                {
                    await ConnectApiService.ReconnectToDcAsync(ex.Dc).ConfigureAwait(false);
                }
            }
        }

        public async Task<TUser> MakeAuthAsync(string phoneNumber, string phoneCodeHash, string code)
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

            var result = (TAuthorization)await SenderService.SendRequestAsync(request).ConfigureAwait(false);

            var user = result.User.Cast<TUser>();

            OnUserAuthenticated(user);

            return user;
        }

        public async Task<IPassword> GetPasswordSetting() => await SenderService.SendRequestAsync(new RequestGetPassword()).ConfigureAwait(false);

        public bool IsUserAuthorized() => ClientSettings.Session.User != null;

        public async Task<TUser> MakeAuthWithPasswordAsync(TPassword password, string passwordStr)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(passwordStr);
            var rv = password.CurrentSalt.Concat(passwordBytes).Concat(password.CurrentSalt);

            byte[] passwordHash;
            using (var sha = SHA256.Create())
            {
                passwordHash = sha.ComputeHash(rv.ToArray());
            }

            var request = new RequestCheckPassword
                          {
                              PasswordHash = passwordHash
                          };
            var result = (TAuthorization)await SenderService.SendRequestAsync(request).ConfigureAwait(false);

            var user = result.User.As<TUser>();

            OnUserAuthenticated(user);

            return user;
        }

        public async Task<TUser> SignUpAsync(string phoneNumber, string phoneCodeHash, string code, string firstName, string lastName)
        {
            var request = new RequestSignUp
                          {
                              PhoneNumber = phoneNumber,
                              PhoneCode = code,
                              PhoneCodeHash = phoneCodeHash,
                              FirstName = firstName,
                              LastName = lastName
                          };
            var result = (TAuthorization)await SenderService.SendRequestAsync(request).ConfigureAwait(false);

            var user = result.User.Cast<TUser>();

            OnUserAuthenticated(user);
            return user;
        }

        private void OnUserAuthenticated(TUser tlUser)
        {
            var session = ClientSettings.Session;
            Guard.That(session).IsNotNull();

            session.User = tlUser;
            session.SessionExpires = int.MaxValue;

            SessionStore.Save();
        }
    }
}