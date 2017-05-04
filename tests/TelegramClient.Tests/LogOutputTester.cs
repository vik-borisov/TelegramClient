namespace TelegramClient.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;

    using TelegramClient.Core;

    using Xunit.Abstractions;

    public class TestOutputAppender : AppenderSkeleton
    {
        private readonly ITestOutputHelper _xunitTestOutputHelper;

        public TestOutputAppender(ITestOutputHelper xunitTestOutputHelper)
        {
            _xunitTestOutputHelper = xunitTestOutputHelper;
            Name = "TestOutputAppender";
            Layout = new PatternLayout("%-5p %d %5rms %-22.22c{1} %-18.18M - %m%n");
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            _xunitTestOutputHelper.WriteLine(RenderLoggingEvent(loggingEvent));
        }
    }

    public class LogOutputTester : IDisposable
    {
        private readonly IAppenderAttachable _attachable;
        private TestOutputAppender _appender;

        protected LogOutputTester(ITestOutputHelper output)
        {
            var repo = LogManager.GetRepository(typeof(ITelegramClient).GetTypeInfo().Assembly);
            XmlConfigurator.Configure(repo, new FileInfo("log4net.config"));
                
            var root = ((log4net.Repository.Hierarchy.Hierarchy)repo).Root;
            _attachable = root;

            _appender = new TestOutputAppender(output);
            _attachable?.AddAppender(_appender);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _attachable.RemoveAppender(_appender);
        }
    }
}