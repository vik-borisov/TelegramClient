namespace TelegramClient.UnitTests.Framework
{
    using CodeProject.ObjectPool;

    using Moq;

    internal static class ObjectPoolMock
    {
        public static Mock<IObjectPool<PooledObjectWrapper<TObject>>> BuildPool<TObject>(this Mock<IObjectPool<PooledObjectWrapper<TObject>>> mock, Mock<TObject> mockObject)
            where TObject : class
        {

            mock
                .Setup(service => service.GetObject())
                .Returns(() => new PooledObjectWrapper<TObject>(mockObject.Object));

            return mock;
        }

        public static Mock<IObjectPool<PooledObjectWrapper<TObject>>> Create<TObject>()
            where TObject : class
        {
            return new Mock<IObjectPool<PooledObjectWrapper<TObject>>>();
        }
    }
}