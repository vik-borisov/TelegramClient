﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TelegramClient.Core;
using OpenTl.Schema;
using Xunit;

namespace TelegramClient.Tests
{
    using log4net;

    using OpenTl.Schema.Account;
    using OpenTl.Schema.Auth;
    using OpenTl.Schema.Contacts;
    using OpenTl.Schema.Messages;
    using OpenTl.Schema.Upload;

    using TelegramClient.Core.Exceptions;
    using TelegramClient.Core.Network.Exceptions;

    using Xunit.Abstractions;

    public class TelegramClientTests: LogOutputTester
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

            Log.Info($"\n\n#################################################  {DateTime.Now}  ################################################################################\n\n");
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

            await client.ConnectService.ConnectAsync();

            var sentCode = (TSentCode) await client.AuthService.SendCodeRequestAsync(NumberToAuthenticate);
            var code = CodeToAuthenticate; // you can change code in debugger too

            if (string.IsNullOrWhiteSpace(code))
                throw new Exception(
                    "CodeToAuthenticate is empty in the appsettings.json file, fill it with the code you just got now by SMS/Telegram");

            TUser user;
            try
            {
                user = await client.AuthService.MakeAuthAsync(NumberToAuthenticate, sentCode.PhoneCodeHash, code);
            }
            catch (CloudPasswordNeededException)
            {
                var password = (TPassword) await client.AuthService.GetPasswordSetting();
                var passwordStr = PasswordToAuthenticate;

                user = await client.AuthService.MakeAuthWithPasswordAsync(password, passwordStr);
            }
            catch (InvalidPhoneCodeException ex)
            {
                throw new Exception(
                    "CodeToAuthenticate is wrong in the appsettings.json file, fill it with the code you just got now by SMS/Telegram",
                    ex);
            }
            Assert.NotNull(user);
            Assert.True(client.AuthService.IsUserAuthorized());
            Thread.Sleep(1000);
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

        private async Task SendMessage(ITelegramClient client, TUser user)
        {
            await client.MessagesService.SendMessageAsync(new TInputPeerUser { UserId = user.Id }, "TEST_" + Random.Next());
        }

        [Fact]
        public virtual async Task GetManualUpdatesTest()
        {
            var client = NewClient();

            await client.ConnectService.ConnectAsync();

            var currentState = await client.UpdatesService.GetCurrentState();

            var user = await GetUser(client);
            await SendMessage(client, user);

            var updates = await client.UpdatesService.GetUpdates(currentState);

            Assert.IsNotType<IEmpty>(updates);
        }

        [Fact]
        public virtual async Task GetAutoUpdatesTest()
        {
            var client = NewClient();

            client.UpdatesService.RecieveUpdates += update =>
            {

            };

            await client.ConnectService.ConnectAsync();
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

            await client.ConnectService.ConnectAsync();

            var user = await GetUser(client);
            await SendMessage(client, user);
        }

        [Fact]
        public virtual async Task SendMessageParallelTest()
        {
            var client = NewClient();

            await client.ConnectService.ConnectAsync();
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

            await client.ConnectService.ConnectAsync();

            await SendMessageToChannel(client);
        }

        private static async Task SendMessageToChannel(ITelegramClient client)
        {
            var dialogs = (TDialogs) await client.MessagesService.GetUserDialogsAsync();

            var chat = dialogs.Chats.Items
                              .OfType<TChannel>()
                              .FirstOrDefault(c => c.Title == "Виктор Борисов");

            await client.MessagesService.SendMessageAsync(
                new TInputPeerChannel
                {
                    ChannelId = chat.Id,
                    AccessHash = chat.AccessHash
                },
                "TEST MSG " + Random.Next());
        }

        [Fact]
        public virtual async Task SendPhotoToContactTest()
        {
            var client = NewClient();

            await client.ConnectService.ConnectAsync();

            var result = await client.ContactsService.GetContactsAsync();

            var user = result.Cast<TContacts>().Users.Items
                .OfType<TUser>()
                .FirstOrDefault(x => x.Phone == NumberToSendMessage);

            using(var stream = new FileStream("data/cat.jpg", FileMode.Open))
            {
                var fileResult = (TInputFile) await client.UploadService.UploadFile("cat.jpg", new StreamReader(stream));
                await client.MessagesService.SendUploadedPhoto(new TInputPeerUser { UserId = user.Id }, fileResult, "kitty");
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

            await client.ConnectService.ConnectAsync();

            var result = await client.ContactsService.GetContactsAsync();

            var user = result.Cast<TContacts>().Users.Items
                .OfType<TUser>()
                .FirstOrDefault(x => x.Phone == NumberToSendMessage);

            var inputPeer = new TInputPeerUser {UserId = user.Id, AccessHash = 0};
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
            var document = res.Messages.Items
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
                });

            Assert.True(resFile.Cast<TFileCdnRedirect>().EncryptionIv.Length > 0);
        }

        [Fact]
        public virtual async Task DownloadFileFromWrongLocationTest()
        {
            var client = NewClient();

            await client.ConnectService.ConnectAsync();

            var result = await client.ContactsService.GetContactsAsync();

            var user = result.Cast<TContacts>().Users.Items
                .OfType<TUser>()
                .FirstOrDefault(x => x.Id == 5880094);

            var photo = (TUserProfilePhoto) user.Photo;
            var photoLocation = (TFileLocation) photo.PhotoBig;

            var resFile = await client.UploadService.GetFile(new TInputFileLocation
            {
                LocalId = photoLocation.LocalId,
                Secret = photoLocation.Secret,
                VolumeId = photoLocation.VolumeId
            });

            var res = await client.MessagesService.GetUserDialogsAsync();

            Assert.True(resFile.Cast<TFile>().Bytes.Length > 0);
        }

        [Fact]
        public virtual async Task SignUpNewUser()
        {
            var client = NewClient();
            await client.ConnectService.ConnectAsync();

            var sentCode = await client.AuthService.SendCodeRequestAsync(NotRegisteredNumberToSignUp);
            var code = "";

            var registeredUser = await client.AuthService.SignUpAsync(NotRegisteredNumberToSignUp, sentCode.PhoneCodeHash, code, "TelegramClient", "User");
            Assert.NotNull(registeredUser);
            Assert.True(client.AuthService.IsUserAuthorized());

            var loggedInUser = await client.AuthService.MakeAuthAsync(NotRegisteredNumberToSignUp, sentCode.PhoneCodeHash, code);
            Assert.NotNull(loggedInUser);
        }

        public virtual async Task CheckPhones()
        {
            var client = NewClient();
            await client.ConnectService.ConnectAsync();

            var result = await client.AuthService.IsPhoneRegisteredAsync(NumberToAuthenticate);
            Assert.True(result.PhoneRegistered);
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

            await client.ConnectService.ConnectAsync();

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
                throw new Exception("Username was not found: " + UserNameToSendMessage);

            await client.MessagesService.SendTypingAsync(new TInputPeerUser {UserId = user.Id});
            Thread.Sleep(3000);
            await client.MessagesService.SendMessageAsync(new TInputPeerUser {UserId = user.Id}, "TEST");
        }
    }
}