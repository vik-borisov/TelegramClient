namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using BarsGroup.CodeGuard;

    using OpenTl.Schema;
    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    using TAuthorization = OpenTl.Schema.Auth.TAuthorization;

    [SingleInstance(typeof(IAuthApiService))]
    internal class AuthApiService : IAuthApiService
    {
        public ISenderService SenderService { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public ISessionStore SessionStore { get; set; }

        public async Task<IPassword> GetPasswordSetting(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SenderService.SendRequestAsync(new RequestGetPassword(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<ICheckedPhone> IsPhoneRegisteredAsync(string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var authCheckPhoneRequest = new RequestCheckPhone
                                        {
                                            PhoneNumber = phoneNumber
                                        };
            return await SenderService.SendRequestAsync(authCheckPhoneRequest, cancellationToken).ConfigureAwait(false);
        }

        public void EnsureUserAuthorized()
        {
            if (!IsUserAuthorized())
            {
                throw new InvalidOperationException("Authorize user first!");
            }
        }

        public bool IsUserAuthorized()
        {
            return ClientSettings.Session.User != null;
        }

        public async Task<TUser> MakeAuthAsync(string phoneNumber, string phoneCodeHash, string code, CancellationToken cancellationToken = default(CancellationToken))
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

            var result = (TAuthorization)await SenderService.SendRequestAsync(request, cancellationToken).ConfigureAwait(false);

            var user = result.User.Cast<TUser>();

            await OnUserAuthenticated(user).ConfigureAwait(false);

            return user;
        }

        public async Task<TUser> MakeAuthWithPasswordAsync(TPassword password, string passwordStr, CancellationToken cancellationToken = default(CancellationToken))
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
            var result = (TAuthorization)await SenderService.SendRequestAsync(request, cancellationToken).ConfigureAwait(false);

            var user = result.User.As<TUser>();

            await OnUserAuthenticated(user).ConfigureAwait(false);

            return user;
        }

        public async Task<ISentCode> SendCodeRequestAsync(string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.That(phoneNumber, nameof(phoneNumber)).IsNotNullOrWhiteSpace();

            var request = new RequestSendCode
                          {
                              PhoneNumber = phoneNumber,
                              ApiId = ClientSettings.AppId,
                              ApiHash = ClientSettings.AppHash
                          };
            return await SenderService.SendRequestAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TUser> SignUpAsync(string phoneNumber, string phoneCodeHash, string code, string firstName, string lastName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new RequestSignUp
                          {
                              PhoneNumber = phoneNumber,
                              PhoneCode = code,
                              PhoneCodeHash = phoneCodeHash,
                              FirstName = firstName,
                              LastName = lastName
                          };
            var result = (TAuthorization)await SenderService.SendRequestAsync(request, cancellationToken).ConfigureAwait(false);

            var user = result.User.Cast<TUser>();

            await OnUserAuthenticated(user).ConfigureAwait(false);
            return user;
        }

        private async Task OnUserAuthenticated(TUser tlUser)
        {
            var session = ClientSettings.Session;
            Guard.That(session).IsNotNull();

            session.User = tlUser;
            session.SessionExpires = int.MaxValue;

            await SessionStore.Save().ConfigureAwait(false);
        }
    }
}