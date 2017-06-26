using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TelegramClient.Core;
using TelegramClient.Core.Network;
using TelegramClient.Core.Utils;
using OpenTl.Schema;
using Xunit;

namespace TelegramClient.Tests
{
    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;

    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.Network.Exceptions;

    using Xunit.Abstractions;

    public class TelegramClientTests: LogOutputTester
    {
        private static readonly Random Random = new Random();

        private string ServerAddress { get; set; }

        private int ServerPort { get; set; }

        private string NumberToSendMessage { get; set; }

        private string NumberToAuthenticate { get; set; }

        private string CodeToAuthenticate { get; set; }

        private string PasswordToAuthenticate { get; set; }

        private string NotRegisteredNumberToSignUp { get; set; }

        private string UserNameToSendMessage { get; set; }

        private string NumberToGetUserFull { get; set; }

        private string NumberToAddToChat { get; set; }

        private string ApiHash { get; set; }

        private int ApiId { get; set; }

        public TelegramClientTests(ITestOutputHelper output) : base(output)
        {
            GatherTestConfiguration();
        }

        private ITelegramClient NewClient()
        {
            try
            {
                return ClientFactory.BuildClient(ApiId, ApiHash, ServerAddress, ServerPort);
            }
            catch (MissingApiConfigurationException ex)
            {
                throw new Exception(
                    $"Please add your API settings to the `appsettings.json` file. (More info: {MissingApiConfigurationException.InfoUrl})",
                    ex);
            }
        }

        private void GatherTestConfiguration()
        {
            var appConfigMsgWarning = "{0} not configured in appsettings.json! Some tests may fail.";

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.debug.json", true)
                .Build();


            ApiHash = builder[nameof(ApiHash)];
            if (string.IsNullOrEmpty(ApiHash))
                Debug.WriteLine(appConfigMsgWarning, nameof(ApiHash));

            var serverPort = builder[nameof(ServerPort)];
            if (string.IsNullOrEmpty(serverPort))
                Debug.WriteLine(appConfigMsgWarning, nameof(ServerPort));
            else
                ServerPort = int.Parse(serverPort);

            ServerAddress = builder[nameof(ServerAddress)];
            if (string.IsNullOrEmpty(ServerAddress))
                Debug.WriteLine(appConfigMsgWarning, nameof(ServerAddress));

            var apiId = builder[nameof(ApiId)];
            if (string.IsNullOrEmpty(apiId))
                Debug.WriteLine(appConfigMsgWarning, nameof(ApiId));
            else
                ApiId = int.Parse(apiId);

            NumberToAuthenticate = builder[nameof(NumberToAuthenticate)];
            if (string.IsNullOrEmpty(NumberToAuthenticate))
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToAuthenticate));

            CodeToAuthenticate = builder[nameof(CodeToAuthenticate)];
            if (string.IsNullOrEmpty(CodeToAuthenticate))
                Debug.WriteLine(appConfigMsgWarning, nameof(CodeToAuthenticate));

            PasswordToAuthenticate = builder[nameof(PasswordToAuthenticate)];
            if (string.IsNullOrEmpty(PasswordToAuthenticate))
                Debug.WriteLine(appConfigMsgWarning, nameof(PasswordToAuthenticate));

            NotRegisteredNumberToSignUp = builder[nameof(NotRegisteredNumberToSignUp)];
            if (string.IsNullOrEmpty(NotRegisteredNumberToSignUp))
                Debug.WriteLine(appConfigMsgWarning, nameof(NotRegisteredNumberToSignUp));

            UserNameToSendMessage = builder[nameof(UserNameToSendMessage)];
            if (string.IsNullOrEmpty(UserNameToSendMessage))
                Debug.WriteLine(appConfigMsgWarning, nameof(UserNameToSendMessage));

            NumberToGetUserFull = builder[nameof(NumberToGetUserFull)];
            if (string.IsNullOrEmpty(NumberToGetUserFull))
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToGetUserFull));

            NumberToAddToChat = builder[nameof(NumberToAddToChat)];
            if (string.IsNullOrEmpty(NumberToAddToChat))
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToAddToChat));

            NumberToSendMessage = builder[nameof(NumberToSendMessage)];
            if (string.IsNullOrEmpty(NumberToSendMessage))
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToSendMessage));
        }

        [Fact]
        public virtual async Task AuthUser()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var sentCode = (TSentCode) await client.SendCodeRequestAsync(NumberToAuthenticate);
            var code = CodeToAuthenticate; // you can change code in debugger too

            if (string.IsNullOrWhiteSpace(code))
                throw new Exception(
                    "CodeToAuthenticate is empty in the appsettings.json file, fill it with the code you just got now by SMS/Telegram");

            TUser user;
            try
            {
                user = await client.MakeAuthAsync(NumberToAuthenticate, sentCode.PhoneCodeHash, code);
            }
            catch (CloudPasswordNeededException)
            {
                var password = (TPassword) await client.GetPasswordSetting();
                var passwordStr = PasswordToAuthenticate;

                user = await client.MakeAuthWithPasswordAsync(password, passwordStr);
            }
            catch (InvalidPhoneCodeException ex)
            {
                throw new Exception(
                    "CodeToAuthenticate is wrong in the appsettings.json file, fill it with the code you just got now by SMS/Telegram",
                    ex);
            }
            Assert.NotNull(user);
            Assert.True(client.IsUserAuthorized());
            Thread.Sleep(1000);
        }

        private async Task<TUser> GetUser(ITelegramClient client)
        {
            var normalizedNumber = NumberToSendMessage.StartsWith("+")
                                       ? NumberToSendMessage.Substring(1, NumberToSendMessage.Length - 1)
                                       : NumberToSendMessage;

            var result = await client.GetContactsAsync();

            return result.Users.Lists
                             .OfType<TUser>()
                             .FirstOrDefault(x => x.Phone == normalizedNumber);
        }

        private async Task SendMessage(ITelegramClient client, TUser user)
        {
            await client.SendMessageAsync(new TInputPeerUser { UserId = user.Id }, "TEST_" + Random.Next());
        }

        [Fact]
        public virtual async Task GetManualUpdatesTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var currentState = await client.GetCurrentState();

            var user = await GetUser(client);
            await SendMessage(client, user);

            var updates = await client.GetUpdates(currentState);

            Assert.IsNotType<TlDifferenceEmpty>(updates);
        }

        [Fact]
        public virtual async Task GetAutoUpdatesTest()
        {
            var client = NewClient();

            client.Updates.RecieveUpdates += update =>
            {

            };

            await client.ConnectAsync();
            var user = await GetUser(client);
            await SendMessage(client, user);
            await SendMessage(client, user);
            await SendMessage(client, user);
            await SendMessage(client, user);

            Thread.Sleep(2000);
        }

        [Fact]
        public virtual async Task SendMessageTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var user = await GetUser(client);
            await SendMessage(client, user);
        }

        [Fact]
        public virtual async Task SendMessageParallelTest()
        {
            var client = NewClient();

            await client.ConnectAsync();
            var user = await GetUser(client);

            var m1 = SendMessage(client, user);
            var m2 = SendMessage(client, user);
            var m3 = SendMessage(client, user);
            var m4 = SendMessage(client, user);
            var m5 = SendMessage(client, user);
            var m6 = SendMessage(client, user);
            var m7 = SendMessage(client, user);
            var m8 = SendMessage(client, user);

            Task.WaitAll(m1, m2, m3, m4, m5, m6, m7, m8);
        }


        [Fact]
        public virtual async Task SendMessageToChannelTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            await SendMessageToChannel(client);
        }

        private static async Task SendMessageToChannel(ITelegramClient client)
        {
            var dialogs = (TlDialogs) await client.GetUserDialogsAsync();

            var chat = dialogs.Chats.Lists
                              .OfType<TlChannel>()
                              .FirstOrDefault(c => c.Title == "Виктор Борисов");

            await client.SendMessageAsync(
                new TlInputPeerChannel
                {
                    ChannelId = chat.Id,
                    AccessHash = chat.AccessHash.Value
                },
                "TEST MSG " + Random.Next());
        }

        [Fact]
        public virtual async Task SendPhotoToContactTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.GetContactsAsync();

            var user = result.Users.Lists
                .OfType<TUser>()
                .FirstOrDefault(x => x.Phone == NumberToSendMessage);

            using(var stream = new FileStream("data/cat.jpg", FileMode.Open))
            {
                var fileResult = (TlInputFile) await client.UploadFile("cat.jpg", new StreamReader(stream));
                await client.SendUploadedPhoto(new TlInputPeerUser { UserId = user.Id }, fileResult, "kitty");
            }
        }

        //[Fact]
        //public virtual async Task SendBigFileToContactTest()
        //{
        //    EnsureNumberToSendMessageSet();

        //    var client = NewClient();

        //    await client.ConnectAsync();

        //    var result = await client.GetContactsAsync();

        //    var user = result.users.lists
        //        .OfType<TLUser>()
        //        .FirstOrDefault(x => x.phone == NumberToSendMessage);

        //    var fileResult =
        //        (TLInputFileBig) await client.UploadFile("some.zip", new StreamReader("<some big file path>"));

        //    await client.SendUploadedDocument(
        //        new TLInputPeerUser {user_id = user.id},
        //        fileResult,
        //        "some zips",
        //        "application/zip",
        //        new TLVector<TLAbsDocumentAttribute>());
        //}

        [Fact]
        public virtual async Task DownloadFileFromContactTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.GetContactsAsync();

            var user = result.Users.Lists
                .OfType<TUser>()
                .FirstOrDefault(x => x.Phone == NumberToSendMessage);

            var inputPeer = new TlInputPeerUser {UserId = user.Id};
            var res = await client.SendRequestAsync<TlMessages>(new TlRequestGetHistory {Peer = inputPeer});
            var document = res.Messages.Lists
                .OfType<TlMessage>()
                .Where(m => m.Media != null)
                .Select(m => m.Media)
                .OfType<TlMessageMediaDocument>()
                .Select(md => md.Document)
                .OfType<TlDocument>()
                .First();

            var resFile = await client.GetFile(
                new TlInputDocumentFileLocation
                {
                    AccessHash = document.AccessHash,
                    Id = document.Id,
                    Version = document.Version
                },
                document.Size);

            Assert.True(resFile.Bytes.Length > 0);
        }

        [Fact]
        public virtual async Task DownloadFileFromWrongLocationTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.GetContactsAsync();

            var user = result.Users.Lists
                .OfType<TUser>()
                .FirstOrDefault(x => x.Id == 5880094);

            var photo = (TUserProfilePhoto) user.Photo;
            var photoLocation = (TlFileLocation) photo.PhotoBig;

            var resFile = await client.GetFile(new TlInputFileLocation
            {
                LocalId = photoLocation.LocalId,
                Secret = photoLocation.Secret,
                VolumeId = photoLocation.VolumeId
            }, 1024);

            var res = await client.GetUserDialogsAsync();

            Assert.True(resFile.Bytes.Length > 0);
        }

        [Fact]
        public virtual async Task SignUpNewUser()
        {
            var client = NewClient();
            await client.ConnectAsync();

            var hash = await client.SendCodeRequestAsync(NotRegisteredNumberToSignUp);
            var code = "";

            var registeredUser = await client.SignUpAsync(NotRegisteredNumberToSignUp, hash, code, "TelegramClient", "User");
            Assert.NotNull(registeredUser);
            Assert.True(client.IsUserAuthorized());

            var loggedInUser = await client.MakeAuthAsync(NotRegisteredNumberToSignUp, hash, code);
            Assert.NotNull(loggedInUser);
        }

        public virtual async Task CheckPhones()
        {
            var client = NewClient();
            await client.ConnectAsync();

            var result = await client.IsPhoneRegisteredAsync(NumberToAuthenticate);
            Assert.True(result);
        }

        [Fact]
        public virtual async Task FloodExceptionShouldNotCauseCannotReadPackageLengthError()
        {
            for (var i = 0; i < 50; i++)
                try
                {
                    await CheckPhones();
                }
                catch (FloodException floodException)
                {
                    Console.WriteLine($"FLOODEXCEPTION: {floodException}");
                    Thread.Sleep(floodException.TimeToWait);
                }
        }

        [Fact]
        public virtual async Task SendMessageByUserNameTest()
        {
            UserNameToSendMessage = Environment.GetEnvironmentVariable(nameof(UserNameToSendMessage));
            if (string.IsNullOrWhiteSpace(UserNameToSendMessage))
                throw new Exception(
                    $"Please fill the '{nameof(UserNameToSendMessage)}' setting in appsettings.json file first");

            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.SearchUserAsync(UserNameToSendMessage);

            var user = result.Users.Lists
                .Where(x => x.GetType() == typeof(TUser))
                .OfType<TUser>()
                .FirstOrDefault(x => x.Username == UserNameToSendMessage.TrimStart('@'));

            if (user == null)
            {
                var contacts = await client.GetContactsAsync();

                user = contacts.Users.Lists
                    .Where(x => x.GetType() == typeof(TUser))
                    .OfType<TUser>()
                    .FirstOrDefault(x => x.Username == UserNameToSendMessage.TrimStart('@'));
            }

            if (user == null)
                throw new Exception("Username was not found: " + UserNameToSendMessage);

            await client.SendTypingAsync(new TlInputPeerUser {UserId = user.Id});
            Thread.Sleep(3000);
            await client.SendMessageAsync(new TlInputPeerUser {UserId = user.Id}, "TEST");
        }
    }
}