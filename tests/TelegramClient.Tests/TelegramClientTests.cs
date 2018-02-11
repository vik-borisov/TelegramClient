namespace TelegramClient.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using Microsoft.Extensions.Configuration;

    using OpenTl.Schema;
    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;
    using OpenTl.Schema.Contacts;
    using OpenTl.Schema.Messages;
    using OpenTl.Schema.Updates;
    using OpenTl.Schema.Upload;

    using TelegramClient.Core;
    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Sessions;

    using Xunit;
    using Xunit.Abstractions;

    public class TelegramClientTests : LogOutputTester
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TelegramClientTests));

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

            Log.Info(
                $"\n\n#################################################  {DateTime.Now}  ################################################################################\n\n");
        }

        [Fact]
        public async Task AuthUser()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var sentCode = (TSentCode)await client.AuthService.SendCodeRequestAsync(NumberToAuthenticate).ConfigureAwait(false);
                var code = CodeToAuthenticate; // you can change code in debugger too

                if (string.IsNullOrWhiteSpace(code))
                {
                    throw new Exception(
                        "CodeToAuthenticate is empty in the appsettings.json file, fill it with the code you just got now by SMS/Telegram");
                }

                TUser user;
                try
                {
                    user = await client.AuthService.MakeAuthAsync(NumberToAuthenticate, sentCode.PhoneCodeHash, code).ConfigureAwait(false);
                }
                catch (CloudPasswordNeededException)
                {
                    var password = (TPassword)await client.AuthService.GetPasswordSetting();
                    var passwordStr = PasswordToAuthenticate;

                    user = await client.AuthService.MakeAuthWithPasswordAsync(password, passwordStr).ConfigureAwait(false);
                }
                catch (InvalidPhoneCodeException ex)
                {
                    throw new Exception(
                        "CodeToAuthenticate is wrong in the appsettings.json file, fill it with the code you just got now by SMS/Telegram",
                        ex);
                }

                Assert.NotNull(user);
                Assert.True(client.AuthService.IsUserAuthorized());
                await Task.Delay(1000);
            }
        }

        [Fact]
        public async Task CheckPhones()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var result = await client.AuthService.IsPhoneRegisteredAsync(NumberToAuthenticate).ConfigureAwait(false);
                Assert.True(result.PhoneRegistered);
            }
        }

        //[Fact]
        //public async Task SendBigFileToContactTest()
        //{
        //    EnsureNumberToSendMessageSet();

        //   using (var client = await NewClient().ConfigureAwait(false))
        //{

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
        public async Task DownloadFileFromContactTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var result = await client.ContactsService.GetContactsAsync().ConfigureAwait(false);

                var user = result.Cast<TContacts>().Users.Items
                                 .OfType<TUser>()
                                 .FirstOrDefault(x => x.Phone == NumberToSendMessage);

                var inputPeer = new TInputPeerUser
                                {
                                    UserId = user.Id,
                                    AccessHash = 0
                                };
                var res = await client.SendService.SendRequestAsync(
                              new RequestGetHistory
                              {
                                  Peer = inputPeer,
                                  Limit = 3,
                                  AddOffset = 0,
                                  MaxId = 0,
                                  MinId = 0,
                                  OffsetDate = 0,
                                  OffsetId = 0
                              });
                var document = res.As<TMessages>().Messages.Items
                                  .OfType<TMessage>()
                                  .Where(m => m.Media != null)
                                  .Select(m => m.Media)
                                  .OfType<TMessageMediaDocument>()
                                  .Select(md => md.Document)
                                  .OfType<TDocument>()
                                  .First();

                var resFile = await client.UploadService.GetFile(
                                  new TInputDocumentFileLocation
                                  {
                                      AccessHash = document.AccessHash,
                                      Id = document.Id,
                                      Version = document.Version
                                  })
                                          .ConfigureAwait(false);

                Assert.True(resFile.Cast<TFileCdnRedirect>().EncryptionIv.Length > 0);
            }
        }

        [Fact]
        public async Task DownloadFileFromWrongLocationTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var result = await client.ContactsService.GetContactsAsync().ConfigureAwait(false);

                var user = result.Cast<TContacts>().Users.Items
                                 .OfType<TUser>()
                                 .FirstOrDefault(x => x.Id == 5880094);

                var photo = (TUserProfilePhoto)user.Photo;
                var photoLocation = (TFileLocation)photo.PhotoBig;

                var resFile = await client.UploadService.GetFile(
                                  new TInputFileLocation
                                  {
                                      LocalId = photoLocation.LocalId,
                                      Secret = photoLocation.Secret,
                                      VolumeId = photoLocation.VolumeId
                                  })
                                          .ConfigureAwait(false);

                await client.MessagesService.GetUserDialogsAsync().ConfigureAwait(false);

                Assert.True(resFile.Cast<TFile>().Bytes.Length > 0);
            }
        }

        [Fact]
        public async Task FloodExceptionShouldNotCauseCannotReadPackageLengthError()
        {
            for (var i = 0; i < 50; i++)
            {
                try
                {
                    await CheckPhones().ConfigureAwait(false);
                }
                catch (FloodException floodException)
                {
                    Console.WriteLine($"FLOODEXCEPTION: {floodException}");
                    await Task.Delay(floodException.TimeToWait).ConfigureAwait(false);
                }
            }
        }

        [Fact]
        public async Task GetAutoUpdatesTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);
                var user = await GetUser(client).ConfigureAwait(false);

                // Register AFTER connecting
                client.UpdatesService.RecieveUpdates += async update => await Task.Delay(1000).ConfigureAwait(false);

                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
                await Task.Delay(5000);
            }
        }

        [Fact]
        public async Task GetManualUpdatesTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var currentState = await client.UpdatesService.GetCurrentState().ConfigureAwait(false);

                var user = await GetUser(client).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);

                var updates = await client.UpdatesService.GetUpdates(currentState).ConfigureAwait(false);

                Assert.IsNotType<TDifferenceEmpty>(updates);
            }
        }

        [Fact]
        public async Task ForLongOperation()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                await GetUser(client).ConfigureAwait(false);

                var tsc = new TaskCompletionSource<bool>();

                await tsc.Task.ConfigureAwait(false);
            }
        }
        
        [Fact]
        public async Task LogOut()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                await GetUser(client).ConfigureAwait(false);

                await client.ConnectService.LogOut().ConfigureAwait(false);

                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                await GetUser(client).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task SendMessageByUserNameTest()
        {
            UserNameToSendMessage = Environment.GetEnvironmentVariable(nameof(UserNameToSendMessage));
            if (string.IsNullOrWhiteSpace(UserNameToSendMessage))
            {
                throw new Exception(
                    $"Please fill the '{nameof(UserNameToSendMessage)}' setting in appsettings.json file first");
            }

            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var result = await client.ContactsService.SearchUserAsync(UserNameToSendMessage);

                var user = result.Users.Items
                                 .Where(x => x.GetType() == typeof(TUser))
                                 .OfType<TUser>()
                                 .FirstOrDefault(x => x.Username == UserNameToSendMessage.TrimStart('@'));

                if (user == null)
                {
                    var contacts = await client.ContactsService.GetContactsAsync();

                    user = contacts.Cast<TContacts>().Users.Items
                                   .Where(x => x.GetType() == typeof(TUser))
                                   .OfType<TUser>()
                                   .FirstOrDefault(x => x.Username == UserNameToSendMessage.TrimStart('@'));
                }

                if (user == null)
                {
                    throw new Exception("Username was not found: " + UserNameToSendMessage);
                }

                await client.MessagesService.SendTypingAsync(new TInputPeerUser { UserId = user.Id });
                await Task.Delay(3000);
                await client.MessagesService.SendMessageAsync(new TInputPeerUser { UserId = user.Id }, "TEST");
            }
        }

        [Fact]
        public async Task SendMessageParallelTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);
                var user = await GetUser(client).ConfigureAwait(false);

                var m1 = SendMessage(client, user);
                var m2 = SendMessage(client, user);
                var m3 = SendMessage(client, user);
                var m4 = SendMessage(client, user);
                var m5 = SendMessage(client, user);
                var m6 = SendMessage(client, user);
                var m7 = SendMessage(client, user);
                var m8 = SendMessage(client, user);

                await Task.WhenAll(m1, m2, m3, m4, m5, m6, m7, m8).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task SendMessageWithCancelTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var user = await GetUser(client).ConfigureAwait(false);

                var cts = new CancellationTokenSource();
                var task = SendMessage(client, user, cts.Token);

                cts.Cancel();
                
                await Assert.ThrowsAsync<TaskCanceledException>(async () => await task.ConfigureAwait(false));
                
                await SendMessage(client, user, CancellationToken.None).ConfigureAwait(false);
            }
        }
        
        [Fact]
        public async Task SendMessageTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var user = await GetUser(client).ConfigureAwait(false);
                await SendMessage(client, user).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task SendMessageToChannelTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                await SendMessageToChannel(client).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task SendPhotoToContactTest()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var result = await client.ContactsService.GetContactsAsync();

                var user = result.Cast<TContacts>().Users.Items
                                 .OfType<TUser>()
                                 .FirstOrDefault(x => x.Phone == NumberToSendMessage);

                using (var stream = new FileStream("data/cat.jpg", FileMode.Open))
                {
                    var fileResult = (TInputFile)await client.UploadService.UploadFile("cat.jpg", new StreamReader(stream));
                    await client.MessagesService.SendUploadedPhoto(new TInputPeerUser { UserId = user.Id }, fileResult, "kitty");
                }
            }
        }

        [Fact]
        public async Task SignUpNewUser()
        {
            using (var client = await NewClient().ConfigureAwait(false))
            {
                await client.ConnectService.ConnectAsync().ConfigureAwait(false);

                var sentCode = await client.AuthService.SendCodeRequestAsync(NotRegisteredNumberToSignUp);
                var code = "";

                var registeredUser = await client.AuthService.SignUpAsync(NotRegisteredNumberToSignUp, sentCode.PhoneCodeHash, code, "TelegramClient", "User");
                Assert.NotNull(registeredUser);
                Assert.True(client.AuthService.IsUserAuthorized());

                var loggedInUser = await client.AuthService.MakeAuthAsync(NotRegisteredNumberToSignUp, sentCode.PhoneCodeHash, code);
                Assert.NotNull(loggedInUser);
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
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(ApiHash));
            }

            var serverPort = builder[nameof(ServerPort)];
            if (string.IsNullOrEmpty(serverPort))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(ServerPort));
            }
            else
            {
                ServerPort = int.Parse(serverPort);
            }

            ServerAddress = builder[nameof(ServerAddress)];
            if (string.IsNullOrEmpty(ServerAddress))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(ServerAddress));
            }

            var apiId = builder[nameof(ApiId)];
            if (string.IsNullOrEmpty(apiId))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(ApiId));
            }
            else
            {
                ApiId = int.Parse(apiId);
            }

            NumberToAuthenticate = builder[nameof(NumberToAuthenticate)];
            if (string.IsNullOrEmpty(NumberToAuthenticate))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToAuthenticate));
            }

            CodeToAuthenticate = builder[nameof(CodeToAuthenticate)];
            if (string.IsNullOrEmpty(CodeToAuthenticate))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(CodeToAuthenticate));
            }

            PasswordToAuthenticate = builder[nameof(PasswordToAuthenticate)];
            if (string.IsNullOrEmpty(PasswordToAuthenticate))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(PasswordToAuthenticate));
            }

            NotRegisteredNumberToSignUp = builder[nameof(NotRegisteredNumberToSignUp)];
            if (string.IsNullOrEmpty(NotRegisteredNumberToSignUp))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(NotRegisteredNumberToSignUp));
            }

            UserNameToSendMessage = builder[nameof(UserNameToSendMessage)];
            if (string.IsNullOrEmpty(UserNameToSendMessage))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(UserNameToSendMessage));
            }

            NumberToGetUserFull = builder[nameof(NumberToGetUserFull)];
            if (string.IsNullOrEmpty(NumberToGetUserFull))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToGetUserFull));
            }

            NumberToAddToChat = builder[nameof(NumberToAddToChat)];
            if (string.IsNullOrEmpty(NumberToAddToChat))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToAddToChat));
            }

            NumberToSendMessage = builder[nameof(NumberToSendMessage)];
            if (string.IsNullOrEmpty(NumberToSendMessage))
            {
                Debug.WriteLine(appConfigMsgWarning, nameof(NumberToSendMessage));
            }
        }

        private async Task<TUser> GetUser(ITelegramClient client)
        {
            var normalizedNumber = NumberToSendMessage.StartsWith("+")
                                       ? NumberToSendMessage.Substring(1, NumberToSendMessage.Length - 1)
                                       : NumberToSendMessage;

            var result = await client.ContactsService.GetContactsAsync();

            return result.Cast<TContacts>().Users.Items
                         .OfType<TUser>()
                         .FirstOrDefault(x => x.Phone == normalizedNumber);
        }

        private async Task<ITelegramClient> NewClient()
        {
            try
            {
                var settings = new FactorySettings
                               {
                                   Hash = ApiHash,
                                   Id = ApiId,
                                   ServerAddress = ServerAddress,
                                   ServerPort = ServerPort,
                                   StoreProvider = new FileSessionStoreProvider("session")
                               };
                return await ClientFactory.BuildClient(settings).ConfigureAwait(false);
            }
            catch (MissingApiConfigurationException ex)
            {
                throw new Exception(
                    $"Please add your API settings to the `appsettings.json` file. (More info: {MissingApiConfigurationException.InfoUrl})",
                    ex);
            }
        }

        private async Task<IUpdates> SendMessage(ITelegramClient client, TUser user, CancellationToken cancellationToken = default (CancellationToken))
        {
           return await client.MessagesService.SendMessageAsync(new TInputPeerUser { UserId = user.Id }, "TEST_" + Random.Next(), cancellationToken);
        }

        private static async Task SendMessageToChannel(ITelegramClient client)
        {
            var dialogs = (TDialogsSlice)await client.MessagesService.GetUserDialogsAsync();

            var chat = dialogs.Chats.Items
                              .OfType<TChannel>()
                              .FirstOrDefault(c => c.Title == "Test");

            await client.MessagesService.SendMessageAsync(
                new TInputPeerChannel
                {
                    ChannelId = chat.Id,
                    AccessHash = chat.AccessHash
                },
                "TEST MSG " + Random.Next());
        }
    }
}