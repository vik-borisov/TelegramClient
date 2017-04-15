using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TelegramClient.Entities
{
    [TlObject(481674261)]
    public class TlVector<T> : TlObject
    {
        public List<T> Lists = new List<T>();

        public override int Constructor => 481674261;

        public override void DeserializeBody(BinaryReader br)
        {
            var count = br.ReadInt32();
            for (var i = 0; i < count; i++)
                if (typeof(T) == typeof(int))
                {
                    Lists.Add((T) Convert.ChangeType(br.ReadInt32(), typeof(T)));
                }
                else if (typeof(T) == typeof(long))
                {
                    Lists.Add((T) Convert.ChangeType(br.ReadInt64(), typeof(T)));
                }
                else if (typeof(T) == typeof(string))
                {
                    Lists.Add((T) Convert.ChangeType(StringUtil.Deserialize(br), typeof(T)));
                }
                else if (typeof(T) == typeof(double))
                {
                    Lists.Add((T) Convert.ChangeType(br.ReadDouble(), typeof(T)));
                }
                else if (typeof(T).GetTypeInfo().BaseType == typeof(TlObject))
                {
                    var constructor = br.ReadInt32();
                    var type = TlContext.GetType(constructor);
                    var obj = Activator.CreateInstance(type);
                    type.GetMethod("DeserializeBody").Invoke(obj, new object[] {br});
                    Lists.Add((T) Convert.ChangeType(obj, type));
                }
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Lists.Count());

            foreach (var item in Lists)
            {
                if (typeof(T) == typeof(int))
                {
                    var res = (int) Convert.ChangeType(item, typeof(int));

                    bw.Write(res);
                }
                else if (typeof(T) == typeof(long))
                {
                    var res = (long) Convert.ChangeType(item, typeof(long));
                    bw.Write(res);
                }
                else if (typeof(T) == typeof(string))
                {
                    var res = (string) Convert.ChangeType(item, typeof(string));
                    StringUtil.Serialize(res, bw);
                }
                else if (typeof(T) == typeof(double))
                {
                    var res = (double) Convert.ChangeType(item, typeof(double));
                    bw.Write(res);
                }
                else if (typeof(T).GetTypeInfo().BaseType == typeof(TlObject))
                {
                    var res = (TlObject) (object) item;
                    res.SerializeBody(bw);
                }
            }
        }
    }
}