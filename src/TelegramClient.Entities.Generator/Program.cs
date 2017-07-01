using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TelegramClient.Entities.Generator
{
    internal class Program
    {
        private static readonly List<string> Keywords = new List<string>(new[]
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "in", "int",
            "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out",
            "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
            "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true",
            "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile",
            "while", "add", "alias", "ascending", "async", "await", "descending", "dynamic", "from", "get", "global",
            "group", "into", "join", "let", "orderby", "partial", "partial", "remove", "select", "set", "value", "var",
            "where", "where", "yield"
        });

        private static readonly List<string> InterfacesList = new List<string>();
        private static readonly List<string> ClassesList = new List<string>();

        public static void Main(string[] args)
        {
            var absStyle = File.ReadAllText("ConstructorAbs.tmp");
            var normalStyle = File.ReadAllText("Constructor.tmp");
            var methodStyle = File.ReadAllText("Method.tmp");
            //string method = File.ReadAllText("constructor.tt");
            var json = "";
            string url;
            if (args.Count() == 0) url = "tl-schema.json";
            else url = args[0];

            json = File.ReadAllText(url);
            var file = File.OpenWrite("Result.cs");
            var sw = new StreamWriter(file);
            var schema = JsonConvert.DeserializeObject<Schema>(json);
            foreach (var c in schema.Constructors)
            {
                InterfacesList.Add(c.Type);
                ClassesList.Add(c.Predicate);
            }
            foreach (var c in schema.Constructors)
            {
                var list = schema.Constructors.Where(x => x.Type == c.Type);
                if (list.Count() > 1)
                {
                    var path = (GetNameSpace(c.Type).Replace("TeleSharp.TL", "TL\\").Replace(".", "") + "\\" +
                                GetNameofClass(c.Type, true) + ".cs").Replace("\\\\", "\\");
                    var classFile = MakeFile(path);
                    using (var writer = new StreamWriter(classFile))
                    {
                        var nspace = GetNameSpace(c.Type)
                            .Replace("TeleSharp.TL", "TL\\")
                            .Replace(".", "")
                            .Replace("\\\\", "\\")
                            .Replace("\\", ".");
                        if (nspace.EndsWith("."))
                            nspace = nspace.Remove(nspace.Length - 1, 1);
                        var temp = absStyle.Replace("/* NAMESPACE */", "TeleSharp." + nspace);
                        temp = temp.Replace("/* NAME */", GetNameofClass(c.Type, true));
                        writer.Write(temp);
                    }
                }
                else
                {
                    InterfacesList.Remove(list.First().Type);
                    list.First().Type = "himself";
                }
            }
            foreach (var c in schema.Constructors)
            {
                var path = (GetNameSpace(c.Predicate).Replace("TeleSharp.TL", "TL\\").Replace(".", "") + "\\" +
                            GetNameofClass(c.Predicate, false) + ".cs").Replace("\\\\", "\\");
                var classFile = MakeFile(path);
                using (var writer = new StreamWriter(classFile))
                {
                    #region About Class

                    var nspace = GetNameSpace(c.Predicate)
                        .Replace("TeleSharp.TL", "TL\\")
                        .Replace(".", "")
                        .Replace("\\\\", "\\")
                        .Replace("\\", ".");
                    if (nspace.EndsWith("."))
                        nspace = nspace.Remove(nspace.Length - 1, 1);
                    var temp = normalStyle.Replace("/* NAMESPACE */", "TeleSharp." + nspace);
                    temp = c.Type == "himself"
                        ? temp.Replace("/* PARENT */", "TLObject")
                        : temp.Replace("/* PARENT */", GetNameofClass(c.Type, true));
                    temp = temp.Replace("/*Constructor*/", c.Id.ToString());
                    temp = temp.Replace("/* NAME */", GetNameofClass(c.Predicate, false));

                    #endregion

                    #region Fields

                    var fields = "";
                    foreach (var tmp in c.Params)
                    {
                        fields +=
                            $"     public {CheckForFlagBase(tmp.Type, GetTypeName(tmp.Type))} {CheckForKeyword(tmp.Name)} " +
                            "{get;set;}" + Environment.NewLine;
                    }
                    temp = temp.Replace("/* PARAMS */", fields);

                    #endregion

                    #region ComputeFlagFunc

                    if (!c.Params.Any(x => x.Name == "flags"))
                    {
                        temp = temp.Replace("/* COMPUTE */", "");
                    }
                    else
                    {
                        var compute = "flags = 0;" + Environment.NewLine;
                        foreach (var param in c.Params.Where(x => IsFlagBase(x.Type)))
                        {
                            if (IsTrueFlag(param.Type))
                                compute +=
                                    $"flags = {CheckForKeyword(param.Name)} ? (flags | {GetBitMask(param.Type)}) : (flags & ~{GetBitMask(param.Type)});" +
                                    Environment.NewLine;
                            else
                                compute +=
                                    $"flags = {CheckForKeyword(param.Name)} != null ? (flags | {GetBitMask(param.Type)}) : (flags & ~{GetBitMask(param.Type)});" +
                                    Environment.NewLine;
                        }
                        temp = temp.Replace("/* COMPUTE */", compute);
                    }

                    #endregion

                    #region SerializeFunc

                    var serialize = "";

                    if (c.Params.Any(x => x.Name == "flags"))
                        serialize += "ComputeFlags();" + Environment.NewLine + "bw.Write(flags);" + Environment.NewLine;
                    foreach (var p in c.Params.Where(x => x.Name != "flags"))
                    {
                        serialize += WriteWriteCode(p) + Environment.NewLine;
                    }
                    temp = temp.Replace("/* SERIALIZE */", serialize);

                    #endregion

                    #region DeSerializeFunc

                    var deserialize = "";

                    foreach (var p in c.Params)
                    {
                        deserialize += WriteReadCode(p) + Environment.NewLine;
                    }
                    temp = temp.Replace("/* DESERIALIZE */", deserialize);

                    #endregion

                    writer.Write(temp);
                }
            }
            foreach (var c in schema.Methods)
            {
                var path = (GetNameSpace(c.method).Replace("TeleSharp.TL", "TL\\").Replace(".", "") + "\\" +
                            GetNameofClass(c.method, false, true) + ".cs").Replace("\\\\", "\\");
                var classFile = MakeFile(path);
                using (var writer = new StreamWriter(classFile))
                {
                    #region About Class

                    var nspace = GetNameSpace(c.method)
                        .Replace("TeleSharp.TL", "TL\\")
                        .Replace(".", "")
                        .Replace("\\\\", "\\")
                        .Replace("\\", ".");
                    if (nspace.EndsWith("."))
                        nspace = nspace.Remove(nspace.Length - 1, 1);
                    var temp = methodStyle.Replace("/* NAMESPACE */", "TeleSharp." + nspace);
                    temp = temp.Replace("/* PARENT */", "TLMethod");
                    temp = temp.Replace("/*Constructor*/", c.Id.ToString());
                    temp = temp.Replace("/* NAME */", GetNameofClass(c.method, false, true));

                    #endregion

                    #region Fields

                    var fields = "";
                    foreach (var tmp in c.Params)
                    {
                        fields +=
                            $"        public {CheckForFlagBase(tmp.Type, GetTypeName(tmp.Type))} {CheckForKeyword(tmp.Name)} " +
                            "{get;set;}" + Environment.NewLine;
                    }
                    fields += $"        public {CheckForFlagBase(c.Type, GetTypeName(c.Type))} Response" +
                              "{ get; set;}" + Environment.NewLine;
                    temp = temp.Replace("/* PARAMS */", fields);

                    #endregion

                    #region ComputeFlagFunc

                    if (!c.Params.Any(x => x.Name == "flags"))
                    {
                        temp = temp.Replace("/* COMPUTE */", "");
                    }
                    else
                    {
                        var compute = "flags = 0;" + Environment.NewLine;
                        foreach (var param in c.Params.Where(x => IsFlagBase(x.Type)))
                        {
                            if (IsTrueFlag(param.Type))
                                compute +=
                                    $"flags = {CheckForKeyword(param.Name)} ? (flags | {GetBitMask(param.Type)}) : (flags & ~{GetBitMask(param.Type)});" +
                                    Environment.NewLine;
                            else
                                compute +=
                                    $"flags = {CheckForKeyword(param.Name)} != null ? (flags | {GetBitMask(param.Type)}) : (flags & ~{GetBitMask(param.Type)});" +
                                    Environment.NewLine;
                        }
                        temp = temp.Replace("/* COMPUTE */", compute);
                    }

                    #endregion

                    #region SerializeFunc

                    var serialize = "";

                    if (c.Params.Any(x => x.Name == "flags"))
                        serialize += "ComputeFlags();" + Environment.NewLine + "bw.Write(flags);" + Environment.NewLine;
                    foreach (var p in c.Params.Where(x => x.Name != "flags"))
                    {
                        serialize += WriteWriteCode(p) + Environment.NewLine;
                    }
                    temp = temp.Replace("/* SERIALIZE */", serialize);

                    #endregion

                    #region DeSerializeFunc

                    var deserialize = "";

                    foreach (var p in c.Params)
                    {
                        deserialize += WriteReadCode(p) + Environment.NewLine;
                    }
                    temp = temp.Replace("/* DESERIALIZE */", deserialize);

                    #endregion

                    #region DeSerializeRespFunc

                    var deserializeResp = "";
                    var p2 = new Param {Name = "Response", Type = c.Type};
                    deserializeResp += WriteReadCode(p2) + Environment.NewLine;
                    temp = temp.Replace("/* DESERIALIZEResp */", deserializeResp);

                    #endregion

                    writer.Write(temp);
                }
            }
        }

        public static string FormatName(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            if (input.IndexOf('.') != -1)
            {
                input = input.Replace(".", " ");
                var temp = "";
                foreach (var s in input.Split(' '))
                {
                    temp += FormatName(s) + " ";
                }
                input = temp.Trim();
            }
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string CheckForKeyword(string name)
        {
            if (Keywords.Contains(name)) return "@" + name;
            return name;
        }

        public static string GetNameofClass(string type, bool isinterface = false, bool ismethod = false)
        {
            if (!ismethod)
                if (type.IndexOf('.') != -1 && type.IndexOf('?') == -1)
                    return isinterface
                        ? "TLAbs" + FormatName(type.Split('.')[1])
                        : "TL" + FormatName(type.Split('.')[1]);
                else if (type.IndexOf('.') != -1 && type.IndexOf('?') != -1)
                    return isinterface
                        ? "TLAbs" + FormatName(type.Split('?')[1])
                        : "TL" + FormatName(type.Split('?')[1]);
                else
                    return isinterface ? "TLAbs" + FormatName(type) : "TL" + FormatName(type);
            if (type.IndexOf('.') != -1 && type.IndexOf('?') == -1)
                return "TLRequest" + FormatName(type.Split('.')[1]);
            if (type.IndexOf('.') != -1 && type.IndexOf('?') != -1)
                return "TLRequest" + FormatName(type.Split('?')[1]);
            return "TLRequest" + FormatName(type);
        }

        private static bool IsFlagBase(string type)
        {
            return type.IndexOf("?") != -1;
        }

        private static int GetBitMask(string type)
        {
            return (int) Math.Pow(2, int.Parse(type.Split('?')[0].Split('.')[1]));
        }

        private static bool IsTrueFlag(string type)
        {
            return type.Split('?')[1] == "true";
        }

        public static string GetNameSpace(string type)
        {
            if (type.IndexOf('.') != -1)
                return "TeleSharp.TL" + FormatName(type.Split('.')[0]);
            return "TeleSharp.TL";
        }

        public static string CheckForFlagBase(string type, string result)
        {
            if (type.IndexOf('?') == -1)
                return result;
            var innerType = type.Split('?')[1];
            if (innerType == "true") return result;
            if (new[] {"bool", "int", "uint", "long", "double"}.Contains(result)) return result + "?";
            return result;
        }

        public static string GetTypeName(string type)
        {
            switch (type.ToLower())
            {
                case "#":
                case "int":
                    return "int";
                case "uint":
                    return "uint";
                case "long":
                    return "long";
                case "double":
                    return "double";
                case "string":
                    return "string";
                case "bytes":
                    return "byte[]";
                case "true":
                case "bool":
                    return "bool";
                case "!x":
                    return "TLObject";
                case "x":
                    return "TLObject";
            }

            if (type.StartsWith("Vector"))
                return "TLVector<" + GetTypeName(type.Replace("Vector<", "").Replace(">", "")) + ">";

            if (type.ToLower().Contains("inputcontact"))
                return "TLInputPhoneContact";


            if (type.IndexOf('.') != -1 && type.IndexOf('?') == -1)
                if (InterfacesList.Any(x => x.ToLower() == type.ToLower()))
                    return FormatName(type.Split('.')[0]) + "." + "TLAbs" + type.Split('.')[1];
                else if (ClassesList.Any(x => x.ToLower() == type.ToLower()))
                    return FormatName(type.Split('.')[0]) + "." + "TL" + type.Split('.')[1];
                else
                    return FormatName(type.Split('.')[1]);
            if (type.IndexOf('?') == -1)
                if (InterfacesList.Any(x => x.ToLower() == type.ToLower()))
                    return "TLAbs" + type;
                else if (ClassesList.Any(x => x.ToLower() == type.ToLower()))
                    return "TL" + type;
                else
                    return type;
            return GetTypeName(type.Split('?')[1]);
        }

        public static string LookTypeInLists(string src)
        {
            if (InterfacesList.Any(x => x.ToLower() == src.ToLower()))
                return "TLAbs" + FormatName(src);
            if (ClassesList.Any(x => x.ToLower() == src.ToLower()))
                return "TL" + FormatName(src);
            return src;
        }

        public static string WriteWriteCode(Param p, bool flag = false)
        {
            switch (p.Type.ToLower())
            {
                case "#":
                case "int":
                    return flag
                        ? $"bw.Write({CheckForKeyword(p.Name)}.Value);"
                        : $"bw.Write({CheckForKeyword(p.Name)});";
                case "long":
                    return flag
                        ? $"bw.Write({CheckForKeyword(p.Name)}.Value);"
                        : $"bw.Write({CheckForKeyword(p.Name)});";
                case "string":
                    return $"StringUtil.Serialize({CheckForKeyword(p.Name)},bw);";
                case "bool":
                    return flag
                        ? $"BoolUtil.Serialize({CheckForKeyword(p.Name)}.Value,bw);"
                        : $"BoolUtil.Serialize({CheckForKeyword(p.Name)},bw);";
                case "true":
                    return $"BoolUtil.Serialize({CheckForKeyword(p.Name)},bw);";
                case "bytes":
                    return $"BytesUtil.Serialize({CheckForKeyword(p.Name)},bw);";
                case "double":
                    return flag
                        ? $"bw.Write({CheckForKeyword(p.Name)}.Value);"
                        : $"bw.Write({CheckForKeyword(p.Name)});";
                default:
                    if (!IsFlagBase(p.Type))
                    {
                        return $"ObjectUtils.SerializeObject({CheckForKeyword(p.Name)},bw);";
                    }
                    else
                    {
                        if (IsTrueFlag(p.Type))
                            return $"";
                        var p2 = new Param {Name = p.Name, Type = p.Type.Split('?')[1]};
                        return $"if ((flags & {GetBitMask(p.Type)}) != 0)" + Environment.NewLine +
                               WriteWriteCode(p2, true);
                    }
            }
        }

        public static string WriteReadCode(Param p)
        {
            switch (p.Type.ToLower())
            {
                case "#":
                case "int":
                    return $"{CheckForKeyword(p.Name)} = br.ReadInt32();";
                case "long":
                    return $"{CheckForKeyword(p.Name)} = br.ReadInt64();";
                case "string":
                    return $"{CheckForKeyword(p.Name)} = StringUtil.Deserialize(br);";
                case "bool":
                case "true":
                    return $"{CheckForKeyword(p.Name)} = BoolUtil.Deserialize(br);";
                case "bytes":
                    return $"{CheckForKeyword(p.Name)} = BytesUtil.Deserialize(br);";
                case "double":
                    return $"{CheckForKeyword(p.Name)} = br.ReadDouble();";
                default:
                    if (!IsFlagBase(p.Type))
                    {
                        if (p.Type.ToLower().Contains("vector"))
                            return
                                $"{CheckForKeyword(p.Name)} = ({GetTypeName(p.Type)})ObjectUtils.DeserializeVector<{GetTypeName(p.Type).Replace("TLVector<", "").Replace(">", "")}>(br);";
                        return $"{CheckForKeyword(p.Name)} = ({GetTypeName(p.Type)})ObjectUtils.DeserializeObject(br);";
                    }
                    else
                    {
                        if (IsTrueFlag(p.Type))
                        {
                            return $"{CheckForKeyword(p.Name)} = (flags & {GetBitMask(p.Type)}) != 0;";
                        }
                        var p2 = new Param {Name = p.Name, Type = p.Type.Split('?')[1]};
                        return $"if ((flags & {GetBitMask(p.Type)}) != 0)" + Environment.NewLine +
                               WriteReadCode(p2) + Environment.NewLine +
                               "else" + Environment.NewLine +
                               $"{CheckForKeyword(p.Name)} = null;" + Environment.NewLine;
                    }
            }
        }

        public static FileStream MakeFile(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (File.Exists(path))
                File.Delete(path);
            return File.OpenWrite(path);
        }
    }
}