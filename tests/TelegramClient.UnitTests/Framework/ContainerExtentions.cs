namespace TelegramClient.UnitTests.Framework
{
    using Autofac;

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

        public static TService Resolve<TService>(this TestBase testBase)
        {
            return testBase.Container.Resolve<TService>();
        }
    }
}