using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TelegramClient.Entities
{
    public static class TlContext
    {
        private static Dictionary<int, Type> Types { get; } = Init();

        public static Dictionary<int, Type> Init()
        {
            return typeof(TlContext).GetTypeInfo().Assembly
                .GetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => t.IsClass && t.Namespace.StartsWith("TelegramClient.Entities"))
                .Where(t => t.IsSubclassOf(typeof(TlObject)))
                .Where(t => t.GetCustomAttribute<TlObjectAttribute>() != null)
                .ToDictionary(x => x.GetCustomAttribute<TlObjectAttribute>().Constructor, x => x.AsType());
        }

        public static Type GetType(int constructor)
        {
            return Types[constructor];
        }
    }
}