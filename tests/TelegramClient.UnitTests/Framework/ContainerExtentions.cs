namespace TelegramClient.UnitTests.Framework
{
    using Autofac;

    using Moq;

    internal static class ContainerExtentions
    {
        public static void RegisterType<TService>(this TestBase testBase)
        {
            testBase.ContainerBuilder.RegisterType<TService>().SingleInstance().PropertiesAutowired();
        }

        public static void RegisterType<TService, TImpl>(this TestBase testBase)
            where TImpl : TService
        {
            testBase.ContainerBuilder.RegisterType<TImpl>().As<TService>().SingleInstance().PropertiesAutowired();
        }

        public static void RegisterInstance<TService, TImpl>(this TestBase testBase, TImpl service)
            where TService : class
            where TImpl : TService
        {
            testBase.ContainerBuilder.RegisterInstance<TService>(service);
        }

        public static void RegisterInstance<TImpl>(this TestBase testBase, TImpl service)
            where TImpl : class 
        {
            testBase.ContainerBuilder.RegisterInstance(service);
        }

        public static void RegisterMock<TObject>(this TestBase testBase, Mock<TObject> mock)
            where TObject : class
        {
            testBase.RegisterInstance(mock);
            testBase.RegisterInstance(mock.Object);
        }

        public static TService Resolve<TService>(this TestBase testBase)
        {
            return testBase.Container.Resolve<TService>();
        }
    }
}