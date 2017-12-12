namespace TelegramClient.Core.IoC
{
    using System;

    using BarsGroup.CodeGuard;

    /// <summary>
    ///     Marks the decorated class as a component that will be available from the service locator / component
    ///     container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ComponentAttribute : Attribute
    {
        public EDependencyLifecycle Lifecycle { get; }

        /// <summary>Type to use for the component registration.</summary>
        public Type[] RegisterAs { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ComponentAttribute" /> class, marking the decorated class as a
        ///     component that will be available from the service locator / component container using the specified
        ///     <paramref name="registerAs" /> type.
        /// </summary>
        /// <param name="registerAs">The type to use to register the decorated component.</param>
        /// <param name="lifecycle">The lifecycle of dependency</param>
        protected ComponentAttribute(Type[] registerAs, EDependencyLifecycle lifecycle)
        {
            Guard.That(registerAs).IsNotEmpty();

            RegisterAs = registerAs;
            Lifecycle = lifecycle;
        }
    }
}