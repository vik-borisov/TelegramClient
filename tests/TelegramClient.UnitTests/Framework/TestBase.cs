﻿namespace TelegramClient.UnitTests.Framework
{
    using Autofac;

    public abstract class TestBase
    {
        public ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

        private IContainer _container;
        public IContainer Container => _container ?? (_container = ContainerBuilder.Build());
    }
}