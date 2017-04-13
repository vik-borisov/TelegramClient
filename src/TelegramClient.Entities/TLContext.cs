using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TelegramClient.Entities
{
    public static class TLContext
    {
        private static Dictionary<int, Type> _types;

        public static void Init()
        {
            _types = typeof(TLContext).GetTypeInfo().Assembly
                .GetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => t.IsClass && t.Namespace.StartsWith("TelegramClient.Entities"))
                .Where(t => t.IsSubclassOf(typeof(TLObject)))
                .Where(t => t.GetCustomAttribute<TLObjectAttribute>() != null)
                .ToDictionary(x => x.GetCustomAttribute<TLObjectAttribute>().Constructor, x => x.AsType());
        }

        public static Type GetType(int Constructor)
        {
            return _types[Constructor];
        }
    }
}