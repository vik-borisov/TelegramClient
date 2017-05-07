namespace TelegramClient.Core.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Features.Scanning;

    /// <summary>
    /// Provides automatic component registration by scanning assemblies and types for
    /// those that have the <see cref="ComponentAttribute"/> annotation.
    /// </summary>
    public static class CompositionExtensions
    {
        /// <summary>
        /// Registers the components found in the given assemblies.
        /// </summary>
        internal static void RegisterAttibuteRegistration(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            RegisterComponents(builder, assemblies.SelectMany(x => x.GetTypes()));
        }

        /// <summary>
        /// Registers the components found in the given set of types.
        /// </summary>
        private static void RegisterComponents(this ContainerBuilder builder, IEnumerable<Type> types)
        {
            var supportedTypes = types.Where(t => t.GetTypeInfo().GetCustomAttribute<ComponentAttribute>(true) != null).ToArray();

            foreach (var type in supportedTypes)
            {
                var attibute = type.GetTypeInfo().GetCustomAttribute<ComponentAttribute>(true);
                builder.RegisterType(type)
                       .As(attibute.RegisterAs)
                       .PropertiesAutowired()
                       .RegisterComponents(attibute.Lifecycle);
            }
        }

        private static void RegisterComponents(this IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder, EDependencyLifecycle lifecycle)
        {
            switch (lifecycle)
            {
                case EDependencyLifecycle.Singleton:
                    builder.SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifecycle), lifecycle, null);
            }
        }
    }
}