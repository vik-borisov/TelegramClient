namespace TelegramClient.Tests.UnitTests
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using TelegramClient.Core.Sessions;

    using Xunit;

    public class MessageIdTests
    {
        [Fact]
        public void IncreaceAndUniqeSequence()
        {
            var session = new Session();

            var lastMessageId = session.GetNewMessageId();

            for (var i = 0; i < 10485730; i++)
            {
                var newMessageId = session.GetNewMessageId();
                
                Assert.True(lastMessageId < newMessageId, $"{lastMessageId} < {newMessageId}");

                lastMessageId = newMessageId;
            }
        }

        [Fact]
        public void IncreaceAndUniqeSequenceParallel()
        {
            var session = new Session();

            var queue = new ConcurrentQueue<long>();

            for (int i = 0; i < 16; i++)
            {
                ThreadPool.QueueUserWorkItem(
                    state =>
                    {
                        Parallel.For(
                            0,
                            16,
                            l =>
                            {
                                for (var j = 0; i < 100000000; i++)
                                {
                                    queue.Enqueue(session.GetNewMessageId());
                                }
                            });
                    });
            }

            queue.TryDequeue(out var previousItem);

            while (queue.Count != 0)
            {
                queue.TryDequeue(out var nextItem);

                Assert.True(previousItem < nextItem, $"{previousItem} < {nextItem}");

                previousItem = nextItem;
            }
        }

        [Fact]
        public void DiviedFour()
        {
            var session = new Session();

            for (var i = 0; i < 1000000; i++)
            {
                var newMessageId = session.GetNewMessageId();

                Assert.Equal(0, newMessageId % 4);
            }
        }
    }
}