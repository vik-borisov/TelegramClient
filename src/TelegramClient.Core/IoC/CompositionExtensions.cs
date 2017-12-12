namespace TelegramClient.Core.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    /// <summary>
    ///     Provides automatic component registration by scanning assemblies and types for those that have the
    ///     <see cref="ComponentAttribute" /> annotation.
    /// </summary>
    public static class CompositionExtensions
    {
        /// <summary>Registers the components found in the given assemblies.</summary>
        internal static void RegisterAttibuteRegistration(this IWindsorContainer builder, params Assembly[] assemblies)
        {
            RegisterComponents(builder, assemblies.SelectMany(x => x.GetTypes()));
        }

        /// <summary>Registers the components found in the given set of types.</summary>
        private static void RegisterComponents(this IWindsorContainer builder, IEnumerable<Type> types)
        {
            var supportedTypes = types.Where(t => t.GetTypeInfo().GetCustomAttribute<ComponentAttribute>(true) != null).ToArray();

            foreach (var type in supportedTypes)
            {
                var attibute = type.GetTypeInfo().GetCustomAttribute<ComponentAttribute>(true);
                builder.Register(Component.For(attibute.RegisterAs).ImplementedBy(type).RegisterComponents(attibute.Lifecycle));
            }
        }

        private static ComponentRegistration<object> RegisterComponents(this ComponentRegistration<object> builder, EDependencyLifecycle lifecycle)
        {
            switch (lifecycle)
            {
                case EDependencyLifecycle.Singleton:
                    builder.LifestyleSingleton();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifecycle), lifecycle, null);
            }

            return builder;
        }
    }
}