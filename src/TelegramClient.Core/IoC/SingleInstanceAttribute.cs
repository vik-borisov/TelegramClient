namespace TelegramClient.Core.IoC
{
    using System;

    public class SingleInstanceAttribute: ComponentAttribute
    {
        public SingleInstanceAttribute(params Type[] registerAs) : base(registerAs, EDependencyLifecycle.Singleton)
        {
        }
    }
}