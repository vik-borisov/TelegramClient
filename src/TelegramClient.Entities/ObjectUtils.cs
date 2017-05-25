using System;
using System.IO;
using System.Reflection;

namespace TelegramClient.Entities
{
    public static class ObjectUtils
    {
        public static object DeserializeObject(BinaryReader reader)
        {
            var constructor = reader.ReadInt32();
            return DeserializeObject(reader, constructor);
        }

        public static object DeserializeObject(BinaryReader reader, int code)
        {
            object obj;
            TypeInfo t;
            try
            {
                t = TlContext.GetType((int)code).GetTypeInfo();
                obj = Activator.CreateInstance(t.AsType());
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Constructor Invalid Or Context.Init Not Called !", ex);
            }
            if (t.IsSubclassOf(typeof(TlMethod)))
            {
                ((TlMethod)obj).DeserializeResponse(reader);
                return obj;
            }
            if (t.IsSubclassOf(typeof(TlObject)))
            {
                ((TlObject)obj).DeserializeBody(reader);
                return obj;
            }
            throw new NotImplementedException("Weird Type : " + t.Namespace + " | " + t.Name);
        }

        public static void SerializeObject(object obj, BinaryWriter writer)
        {
            ((TlObject) obj).SerializeBody(writer);
        }

        public static TlVector<T> DeserializeVector<T>(BinaryReader reader)
        {
            if (reader.ReadInt32() != 481674261) throw new InvalidDataException("Bad Constructor");
            var t = new TlVector<T>();
            t.DeserializeBody(reader);
            return t;
        }
    }
}