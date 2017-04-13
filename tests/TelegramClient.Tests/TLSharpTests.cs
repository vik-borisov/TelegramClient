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
using TelegramClient.Entities.TL;
using TelegramClient.Entities.TL.Messages;
using Xunit;

namespace TelegramClient.Tests
{
    public class TLSharpTests
    {
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

        public TLSharpTests()
        {
            GatherTestConfiguration();
        }

        private TClient NewClient()
        {
            try
            {
                return new TClient(ApiId, ApiHash);
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

            var hash = await client.SendCodeRequestAsync(NumberToAuthenticate);
            var code = CodeToAuthenticate; // you can change code in debugger too

            if (string.IsNullOrWhiteSpace(code))
                throw new Exception(
                    "CodeToAuthenticate is empty in the appsettings.json file, fill it with the code you just got now by SMS/Telegram");

            TLUser user = null;
            try
            {
                user = await client.MakeAuthAsync(NumberToAuthenticate, hash, code);
            }
            catch (CloudPasswordNeededException ex)
            {
                var password = await client.GetPasswordSetting();
                var password_str = PasswordToAuthenticate;

                user = await client.MakeAuthWithPasswordAsync(password, password_str);
            }
            catch (InvalidPhoneCodeException ex)
            {
                throw new Exception(
                    "CodeToAuthenticate is wrong in the appsettings.json file, fill it with the code you just got now by SMS/Telegram",
                    ex);
            }
            Assert.NotNull(user);
            Assert.True(client.IsUserAuthorized());
        }

        [Fact]
        public virtual async Task SendMessageTest()
        {
            // this is because the contacts in the address come without the "+" prefix
            var normalizedNumber = NumberToSendMessage.StartsWith("+")
                ? NumberToSendMessage.Substring(1, NumberToSendMessage.Length - 1)
                : NumberToSendMessage;

            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.GetContactsAsync();

            var user = result.users.lists
                .OfType<TLUser>()
                .FirstOrDefault(x => x.phone == normalizedNumber);

            if (user == null)
                throw new Exception("Number was not found in Contacts List of user: " + NumberToSendMessage);

            await client.SendTypingAsync(new TLInputPeerUser {user_id = user.id});
            Thread.Sleep(3000);
            await client.SendMessageAsync(new TLInputPeerUser {user_id = user.id}, "TEST");
        }

        [Fact]
        public virtual async Task SendMessageToChannelTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var dialogs = (TLDialogs) await client.GetUserDialogsAsync();
            var chat = dialogs.chats.lists
                .OfType<TLChannel>()
                .FirstOrDefault(c => c.title == "TestGroup");

            await client.SendMessageAsync(
                new TLInputPeerChannel {channel_id = chat.id, access_hash = chat.access_hash.Value}, "TEST MSG");
        }

        [Fact]
        public virtual async Task SendPhotoToContactTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.GetContactsAsync();

            var user = result.users.lists
                .OfType<TLUser>()
                .FirstOrDefault(x => x.phone == NumberToSendMessage);

            using(var stream = new FileStream("data/cat.jpg", FileMode.Open))
            {
                var fileResult = (TLInputFile) await client.UploadFile("cat.jpg", new StreamReader(stream));
                await client.SendUploadedPhoto(new TLInputPeerUser { user_id = user.id }, fileResult, "kitty");
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

            var user = result.users.lists
                .OfType<TLUser>()
                .FirstOrDefault(x => x.phone == NumberToSendMessage);

            var inputPeer = new TLInputPeerUser {user_id = user.id};
            var res = await client.SendRequestAsync<TLMessagesSlice>(new TLRequestGetHistory {peer = inputPeer});
            var document = res.messages.lists
                .OfType<TLMessage>()
                .Where(m => m.media != null)
                .Select(m => m.media)
                .OfType<TLMessageMediaDocument>()
                .Select(md => md.document)
                .OfType<TLDocument>()
                .First();

            var resFile = await client.GetFile(
                new TLInputDocumentFileLocation
                {
                    access_hash = document.access_hash,
                    id = document.id,
                    version = document.version
                },
                document.size);

            Assert.True(resFile.bytes.Length > 0);
        }

        [Fact]
        public virtual async Task DownloadFileFromWrongLocationTest()
        {
            var client = NewClient();

            await client.ConnectAsync();

            var result = await client.GetContactsAsync();

            var user = result.users.lists
                .OfType<TLUser>()
                .FirstOrDefault(x => x.id == 5880094);

            var photo = (TLUserProfilePhoto) user.photo;
            var photoLocation = (TLFileLocation) photo.photo_big;

            var resFile = await client.GetFile(new TLInputFileLocation
            {
                local_id = photoLocation.local_id,
                secret = photoLocation.secret,
                volume_id = photoLocation.volume_id
            }, 1024);

            var res = await client.GetUserDialogsAsync();

            Assert.True(resFile.bytes.Length > 0);
        }

        [Fact]
        public virtual async Task SignUpNewUser()
        {
            var client = NewClient();
            await client.ConnectAsync();

            var hash = await client.SendCodeRequestAsync(NotRegisteredNumberToSignUp);
            var code = "";

            var registeredUser = await client.SignUpAsync(NotRegisteredNumberToSignUp, hash, code, "TLSharp", "User");
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

            var user = result.users.lists
                .Where(x => x.GetType() == typeof(TLUser))
                .OfType<TLUser>()
                .FirstOrDefault(x => x.username == UserNameToSendMessage.TrimStart('@'));

            if (user == null)
            {
                var contacts = await client.GetContactsAsync();

                user = contacts.users.lists
                    .Where(x => x.GetType() == typeof(TLUser))
                    .OfType<TLUser>()
                    .FirstOrDefault(x => x.username == UserNameToSendMessage.TrimStart('@'));
            }

            if (user == null)
                throw new Exception("Username was not found: " + UserNameToSendMessage);

            await client.SendTypingAsync(new TLInputPeerUser {user_id = user.id});
            Thread.Sleep(3000);
            await client.SendMessageAsync(new TLInputPeerUser {user_id = user.id}, "TEST");
        }
    }
}