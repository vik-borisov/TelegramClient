namespace TelegramClient.Core.Sessions
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    using NullGuard;

    public class FileSessionStoreProvider : ISessionStoreProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileSessionStoreProvider));

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private readonly string _sessionFile;

        private FileStream _fileStream;

        public FileSessionStoreProvider(string sessionTag)
        {
            _sessionFile = $"{sessionTag}.dat";
        }

        [return:AllowNull]
        public async Task<byte[]> LoadSession()
        {
            Log.Debug($"Load session for sessionTag = {_sessionFile}");

            await EnsureStreamOpen();

            var buffer = new byte[2048];

            await _semaphore.WaitAsync();

            _fileStream.Position = 0;

            if (_fileStream.Length == 0)
            {
                _semaphore.Release();

                return null;
            }

            await _fileStream.ReadAsync(buffer, 0, 2048).ConfigureAwait(false);

            _semaphore.Release();

            return buffer;
        }

        public Task RemoveSession()
        {
            if (File.Exists(_sessionFile))
            {
                _fileStream.Dispose();
                _fileStream = null;

                File.Delete(_sessionFile);
            }

            return Task.FromResult(true);
        }

        public async Task SaveSession(byte[] session)
        {
            Log.Debug($"Save session into {_sessionFile}");

            await EnsureStreamOpen();

            await _semaphore.WaitAsync().ConfigureAwait(false);

            _fileStream.Position = 0;
            await _fileStream.WriteAsync(session, 0, session.Length).ConfigureAwait(false);
            await _fileStream.FlushAsync().ConfigureAwait(false);

            _semaphore.Release();
        }

        private async Task EnsureStreamOpen()
        {
            if (_fileStream == null)
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                if (_fileStream == null)
                {
                    _fileStream = new FileStream(_sessionFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }

                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
            _fileStream?.Dispose();
        }
    }
}