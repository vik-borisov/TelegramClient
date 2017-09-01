namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;

    public interface IAuthApiService
    {
        Task<ICheckedPhone> IsPhoneRegisteredAsync(string phoneNumber);

        Task<ISentCode> SendCodeRequestAsync(string phoneNumber);

        Task<TUser> MakeAuthAsync(string phoneNumber, string phoneCodeHash, string code);

        Task<IPassword> GetPasswordSetting();

        bool IsUserAuthorized();

        Task<TUser> MakeAuthWithPasswordAsync(TPassword password, string passwordStr);

        Task<TUser> SignUpAsync(string phoneNumber, string phoneCodeHash, string code, string firstName, string lastName);
    }
}