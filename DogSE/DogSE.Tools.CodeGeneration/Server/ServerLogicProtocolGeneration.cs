using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using DogSE.Library.Log;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Task;

namespace DogSE.Tools.CodeGeneration.Server
{


        /// <summary>
        /// 消息自动创建接口
        /// </summary>
        public interface IProtoclAutoCode
        {
            /// <summary>
            /// 初始化数据
            /// </summary>
            void Init();

            /// <summary>
            /// 消息管理器
            /// </summary>
            PacketHandlersBase PacketHandlerManager { get; set; }

            /// <summary>
            /// 设置模块对象
            /// </summary>
            /// <param name="module"></param>
            void SetModule(ILogicModule module);
        }

        /// <summary>
        /// 访问代码创建
        /// </summary>
        class CreateReadCode
        {
            private readonly Type classType;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="type">要创建的方法的类</param>
            public CreateReadCode(Type type)
            {
                classType = type;
            }

            private readonly StringBuilder initCode = new StringBuilder();
            private readonly StringBuilder callCode = new StringBuilder();

            /// <summary>
            /// 读取代理类
            /// </summary>
            private readonly StringBuilder readProxyCode = new StringBuilder();


            private HashSet<Type> readProxySet = new HashSet<Type>();

            /// <summary>
            /// 添加一个读取代理类
            /// </summary>
            /// <param name="type"></param>
            private void AddReadProxy(Type type)
            {
                if (readProxySet.Contains(type))
                    return;

                readProxySet.Add(type);

                StringBuilder readCode = new StringBuilder();

                foreach(var p in type.GetProperties())
                {
                    if (!p.CanRead || !p.CanWrite)
                        continue;

                    if (p.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                        continue;

                    if (p.PropertyType == typeof(int))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadInt32();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(byte))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadByte();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(long))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadLong64();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(float))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadFloat();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(double))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadFloat();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(bool))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadBoolean();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(string))
                    {
                        readCode.AppendFormat("ret.{0} = reader.ReadUTF8String();\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(DateTime))
                    {
                        readCode.AppendFormat("ret.{0} = new DateTime(reader.ReadLong64());\r\n", p.Name);
                    }
                    else if (p.PropertyType.IsEnum)
                    {
                        readCode.AppendFormat("ret.{0} = ({1})reader.ReadByte();\r\n", p.Name, p.PropertyType.FullName);
                    }
                    else if (p.PropertyType.IsLayoutSequential)
                    {
                        //readCode.AppendFormat("ret.{0} = reader.ReadStruct <{1}>();\r\n", p.Name, p.PropertyType.FullName);

                        AddReadProxyByStruct(p.PropertyType);
                        readCode.AppendFormat("ret.{0} = {1}ReadProxy.Read(reader);\r\n",p.Name, p.PropertyType.Name);
                    }
                    else if (p.PropertyType.IsClass)
                    {
                        //readCode.AppendFormat("ret.{0} = reader.ReadStruct <{1}>();\r\n", p.Name, p.PropertyType.FullName);

                        AddReadProxy(p.PropertyType);
                        readCode.AppendFormat("ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.PropertyType.Name);
                    }
                    else
                    {
                        Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                            classType.Name, type.Name, p.Name, p.PropertyType.Name));
                    }
                }

                readProxyCode.AppendLine(
                    readProxyCodeFormatter.Replace("#TypeName#", type.Name)
                    .Replace("#TypeFullName#", type.FullName)
                    .Replace("#ReadCode#", readCode.ToString())
                    );
            }

        /// <summary>
        /// 添加一个读取代理类
        /// </summary>
        /// <param name="type"></param>
        private void AddReadProxyByStruct(Type type)
        {
            if (readProxySet.Contains(type))
                return;

            readProxySet.Add(type);

            StringBuilder readCode = new StringBuilder();

            foreach (var p in type.GetFields())
            {
                if (p.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                    continue;

                if (p.FieldType == typeof(int))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadInt32();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(byte))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadByte();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(long))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadLong64();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(float))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadFloat();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(double))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadFloat();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(bool))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadBoolean();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(string))
                {
                    readCode.AppendFormat("ret.{0} = reader.ReadUTF8String();\r\n", p.Name);
                }
                else if (p.FieldType == typeof(DateTime))
                {
                    readCode.AppendFormat("ret.{0} = new DateTime(reader.ReadLong64());\r\n", p.Name);
                }
                else if (p.FieldType.IsEnum)
                {
                    readCode.AppendFormat("ret.{0} = ({1})reader.ReadByte();\r\n", p.Name, p.FieldType.FullName);
                }
                else if (p.FieldType.IsLayoutSequential)
                {
                    //readCode.AppendFormat("ret.{0} = reader.ReadStruct <{1}>();\r\n", p.Name, p.FieldType.FullName);
                    AddReadProxyByStruct(p.FieldType);
                    readCode.AppendFormat("ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.FieldType.Name);
                }
                else if (p.FieldType.IsClass)
                {
                    AddReadProxy(p.FieldType);
                    readCode.AppendFormat("ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.FieldType.Name);
                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.FieldType.Name));
                }
            }

            readProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", type.Name)
                .Replace("#TypeFullName#", type.FullName)
                .Replace("#ReadCode#", readCode.ToString())
                );
        }

        private string readProxyCodeFormatter = @"

        /// <summary>
        /// 
        /// </summary>
    public class #TypeName#ReadProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=`reader`></param>
        /// <returns></returns>
        public static #TypeFullName# Read(PacketReader reader)
        {
            #TypeFullName# ret = new #TypeFullName#();

#ReadCode#

            return ret;
        }
    }";

            /// <summary>
            /// 添加一个方法
            /// </summary>
            /// <param name="att"></param>
            /// <param name="methodinfo"></param>
            void AddMethod(NetMethodAttribute att, MethodInfo methodinfo)
            {
                var param = methodinfo.GetParameters();
                if (param.Length < 1)
                {
                    Logs.Error(string.Format("{0}.{1} 不支持 {2} 个参数", classType.Name, methodinfo.Name, param.Length.ToString()));
                    return;
                }

                if (param[0].ParameterType != typeof(NetState))
                {
                    if (!att.IsVerifyLogin)
                    {
                        //  如果不需要验证登录数据，则第一个对象必须是NetState对象
                        Logs.Error("{0}.{1} 的第一个参数必须是 NetState 对象", classType.Name, methodinfo.Name);
                        return;
                    }

                    var componentType = param[0].ParameterType;
                    var field = componentType.GetField("ComponentId");
                    if (field == null || !field.IsStatic)
                    {
                        Logs.Error("{0}.{1} 必须包含一个 ComponentId 的常量字符串作为登录验证后和NetState绑定的组件。");
                        return;
                    }


                }

                if (att.MethodType == NetMethodType.PacketReader)
                {
                    if (param[1].ParameterType != typeof(PacketReader))
                    {
                        Logs.Error("{0}.{1} 的第二个参数必须是 PacketReader 对象", classType.Name, methodinfo.Name);
                        return;
                    }

                    //string mehtodName = methodinfo.Name;

                    if (att.TaskType != TaskType.Main)
                    {
                        initCode.AppendFormat("PacketHandlerManager.Register({0}, TaskType.{2}, module.{1});",
                      att.OpCode, methodinfo.Name, att.TaskType);

                    }
                    initCode.AppendFormat("PacketHandlerManager.Register({0}, module.{1});",
                                          att.OpCode, methodinfo.Name);
                    initCode.AppendLine();

                    //callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", mehtodName);
                    //callCode.AppendLine("{");
                    //callCode.AppendFormat("module.{0}(netstate, reader);", methodinfo.Name);
                    //callCode.AppendLine("}");
                }

                if (att.MethodType == NetMethodType.ProtocolStruct)
                {
                    if (!param[1].ParameterType.IsClass)
                    {
                        Logs.Error("{0}.{1} 的第二个参数必须是class类型。", classType.Name, methodinfo.Name);
                        return;
                    }

                    if (param[1].ParameterType.GetInterface(typeof (IPacketReader).FullName) == null)
                    {
                        Logs.Error("{0}.{1} 的第二个参数必须实现 IPacketReader 接口", classType.Name, methodinfo.Name);
                        //  自己实现一个对对象的协议读取类
                        AddReadProxy(param[1].ParameterType);
                        string methodName = methodinfo.Name;
                        initCode.AppendFormat("PacketHandlerManager.Register({0}, {1});",
                                              att.OpCode, methodName);
                        initCode.AppendLine();

                        callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", methodName);
                        callCode.AppendLine("{");
                        callCode.AppendFormat(" var obj = {0}.Read(reader)", param[1].ParameterType.Name);
                        callCode.AppendFormat("module.{0}(netstate, obj);", methodinfo.Name);
                        callCode.AppendLine("}");
                    }
                    else
                    {
                        //  如果对象实现了 IPacketReader 接口，则直接使用，否则则自己生成协议代码
                        string methodName = methodinfo.Name;
                        initCode.AppendFormat("PacketHandlerManager.Register({0}, {1});",
                                              att.OpCode, methodName);
                        initCode.AppendLine();

                        callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", methodName);
                        callCode.AppendLine("{");
                        callCode.AppendFormat(" var package = DogSE.Library.Common.StaticObjectPool<{0}>.AcquireContent();", param[1].ParameterType.FullName);
                        callCode.AppendLine("package.Read(reader);");
                        callCode.AppendFormat("module.{0}(netstate, package);", methodinfo.Name);
                        callCode.AppendFormat("DogSE.Library.Common.StaticObjectPool<{0}>.ReleaseContent(package);", param[1].ParameterType.FullName);
                        callCode.AppendLine("}");
                    }

                }

                if (att.MethodType == NetMethodType.SimpleMethod)
                {
                    string methodName = methodinfo.Name;

                    if (att.TaskType != TaskType.Main)
                    {
                        initCode.AppendFormat("PacketHandlerManager.Register({0}, TaskType.{2}, {1});",
                            att.OpCode, methodinfo.Name, att.TaskType);

                    }
                    else
                    {
                        initCode.AppendFormat("PacketHandlerManager.Register({0}, {1});",
                            att.OpCode, methodName);
                    }
                    initCode.AppendLine();


                    callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", methodName);
                    callCode.AppendLine("{");
                    if (att.IsVerifyLogin)
                        callCode.AppendLine("if (!netstate.IsVerifyLogin) return;");

                    for (int i = 1; i < param.Length; i++)
                    {
                        var p = param[i];
                        if (p.ParameterType == typeof (int))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadInt32();\r\n", i);
                        }
                        else if (p.ParameterType == typeof (byte))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadByte();\r\n", i);
                        }
                        else if (p.ParameterType == typeof (long))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadLong64();\r\n", i);
                        }
                        else if (p.ParameterType == typeof (float))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadFloat();\r\n", i);
                        }
                        else if (p.ParameterType == typeof (double))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadFloat();\r\n", i);
                        }
                        else if (p.ParameterType == typeof (bool))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadBoolean();\r\n", i);
                        }
                        else if (p.ParameterType == typeof (string))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadUTF8String();\r\n", i);
                        }
                        else if (p.ParameterType == typeof(DateTime))
                        {
                            callCode.AppendFormat("var p{0} = new DateTime(reader.ReadLong64());\r\n", i);
                        }
                        else if (p.ParameterType.IsEnum)
                        {
                            callCode.AppendFormat("var p{0} = ({1})reader.ReadByte();\r\n", i, p.ParameterType.FullName);
                        }
                        else if (p.ParameterType.IsLayoutSequential)
                        {
                        AddReadProxyByStruct(p.ParameterType);
                        callCode.AppendFormat(" var p{1} = {0}ReadProxy.Read(reader);\r\n", p.ParameterType.Name, i);

                        //callCode.AppendFormat("var p{0} = reader.ReadStruct <{1}>();\r\n", i, p.ParameterType.FullName);
                    }
                    else if (p.ParameterType.IsArray)
                        {
                            //  数组
                            #region 处理数组的读取
                            var arrayType = p.ParameterType.GetElementType();

                            callCode.AppendFormat("var len{0} = reader.ReadInt32();\r\n", i);
                            callCode.AppendFormat("var p{0} = new {1}[len{0}];", i, arrayType.FullName);   //  这里只创建值类型
                            
                            callCode.AppendFormat("for(int i =0;i< len{0};i++){{\r\n", i);
                            if (arrayType == typeof(int))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadInt32();\r\n", i);
                            }
                            else if (arrayType == typeof(byte))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadByte();\r\n", i);
                            }
                            else if (arrayType == typeof(long))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadLong64();\r\n", i);
                            }
                            else if (arrayType == typeof(float))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadFloat();\r\n", i);
                            }
                            else if (arrayType == typeof(double))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadFloat();\r\n", i);
                            }
                            else if (arrayType == typeof(bool))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadBoolean();\r\n", i);
                            }
                            else if (arrayType == typeof(string))
                            {
                                callCode.AppendFormat("p{0}[i] = reader.ReadUTF8String();\r\n", i);
                            }
                            else if (arrayType == typeof(DateTime))
                            {
                                callCode.AppendFormat("p{0}[i] =  new DateTime(reader.ReadLong64());\r\n", i);
                            }
                            else if (arrayType.IsEnum)
                            {
                                callCode.AppendFormat("p{0}[i] = ({1})reader.ReadByte();\r\n", i, p.ParameterType.FullName);
                            }
                            else if (arrayType.IsLayoutSequential)
                            {
                            //callCode.AppendFormat("p{0}[i] = reader.ReadStruct <{1}>();\r\n", i, p.ParameterType.FullName);
                            AddReadProxyByStruct(arrayType);
                            callCode.AppendFormat("p{1}[i] = {0}ReadProxy.Read(reader);\r\n", arrayType.Name, i);
                        }
                            else if (arrayType.IsClass)
                            {
                                AddReadProxy(arrayType);
                                callCode.AppendFormat("p{1}[i] = {0}ReadProxy.Read(reader);\r\n", arrayType.Name, i);
                            }

                            callCode.AppendLine("}");

                            #endregion
                        }
                        else if (p.ParameterType.IsClass)
                        {
                            AddReadProxy(p.ParameterType);
                            callCode.AppendFormat(" var p{1} = {0}ReadProxy.Read(reader);\r\n", p.ParameterType.Name, i);
                        }
                        else
                        {
                            Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                                classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                        }

                    }

                    if (param[0].ParameterType != typeof(NetState) && att.IsVerifyLogin)
                    {
                        //  作为验证数据
                        //var componentType = param[0].ParameterType;
                        //callCode.AppendFormat("var {0} = netstate.GetComponent<{1}>({1}.ComponentId);",
                        //   componentType.Name.ToLower(), componentType.Name);

                        //callCode.AppendFormat("module.{0}({1}", methodinfo.Name, componentType.Name.ToLower());

                        callCode.AppendFormat("module.{0}(netstate", methodinfo.Name);
                    }
                    else
                    {
                        //  不需要验证
                        callCode.AppendFormat("module.{0}(netstate", methodinfo.Name);
                    }


                    for (int i = 1; i < param.Length; i++)
                        callCode.AppendFormat(",p{0}", i);
                    callCode.AppendLine(");");
                    callCode.AppendLine("}");

                }
            }

            private static int s_version = 1;


            readonly int Version = s_version++;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            string GetCode()
            {
                var ret = new StringBuilder();

                ret.Append(CodeBase);
                ret.Replace("#ClassName#", classType.Name);
                ret.Replace("#FullClassName#", classType.FullName);
                ret.Replace("#version#", Version.ToString());
                ret.Replace("#InitMethod#", initCode.ToString());
                ret.Replace("#CallMethod#", callCode.ToString());
                ret.Replace("#ReadProxy#", readProxyCode.ToString());
                ret.Replace("#using#", "");
                ret.Replace("`", "\"");

                return ret.ToString();
            }

            /// <summary>
            /// 创建代码并进行编译
            /// </summary>
            /// <returns></returns>
            public string CreateCode()
            {
                foreach (var method in classType.GetMethods())
                {
                    var attributes = method.GetCustomAttributes(typeof(NetMethodAttribute), true);
                    if (attributes.Length == 0)
                        continue;

                    AddMethod(attributes[0] as NetMethodAttribute, method);
                }

                var code = GetCode();
                //Console.WriteLine(code);
                return code;
            }

            /// <summary>
            /// 获得代理注册代码
            /// </summary>
            /// <returns></returns>
            public string CreateRegisterProxyCode()
            {
                StringBuilder ret = new StringBuilder();
                ret.Append(@"                if (m is #FullClassName#)
                {
                    IProtoclAutoCode pac = new #ClassName#Access#version#();
                    list.Add(pac);

                    pac.SetModule(m as #FullClassName#);
                    pac.PacketHandlerManager = handlers;
                    pac.Init();
                }");

                ret.Replace("#ClassName#", classType.Name);
                ret.Replace("#FullClassName#", classType.FullName);
                ret.Replace("#version#", Version.ToString());
                ret.Replace("#InitMethod#", initCode.ToString());
                ret.Replace("#CallMethod#", callCode.ToString());
                ret.Replace("#using#", "");
                ret.Replace("`", "\"");

                return ret.ToString();

            }

            /// <summary>
            /// 编译后生成的组件
            /// </summary>
            public Assembly CompiledAssembly { get; set; }

            private const string CodeBase = @"

    class #ClassName#Access#version#:IProtoclAutoCode
    {
        public PacketHandlersBase PacketHandlerManager {get;set;}

        #FullClassName# module;

        public void SetModule(ILogicModule m)
        {
            if (m == null)
                throw new ArgumentNullException(`ILogicModule`);
            module = (#FullClassName#)m;
            if (module == null)
            {
                throw new NullReferenceException(string.Format(`{0} not #FullClassName#`, m.GetType().FullName));
            }
        }


        public void Init()
        {
#InitMethod#
        }

#CallMethod#

#ReadProxy#
    }
";
        }
    

    /// <summary>
    /// 服务器代理代码创建
    /// </summary>
    class ServerLogicProtocolGeneration
    {
        /// <summary>
        /// 创建代码
        /// </summary>
        /// <param name="dllFile"></param>
        /// <param name="outFile"></param>
        public static void CreateCode(string dllFile, string outFile)
        {
            StringBuilder codeBuilder = new StringBuilder();

            var dll = Assembly.LoadFrom(dllFile);
            StringBuilder proxyregBuilder = new StringBuilder();

            foreach(var type in dll.GetTypes())
            {
                if (type.IsInterface)
                {
                    var i = type.GetInterface("DogSE.Server.Core.LogicModule.ILogicModule");
                    if (i != null)
                    {
                        Console.WriteLine(type.ToString());

                        var crc = new CreateReadCode(type);
                        codeBuilder.Append(crc.CreateCode());
                        proxyregBuilder.Append(crc.CreateRegisterProxyCode());
                    }
                }
            }

            var fileContext = FileCodeBase
                .Replace("#code#", codeBuilder.ToString())
                .Replace("#proxyregister#", proxyregBuilder.ToString())
                .Replace("`", "\"");

            File.WriteAllText(outFile, fileContext, Encoding.UTF8);
        }


        void test()
        {
            CreateCode(@"E:\Project\DogSE\TradeAge\TradeAge.Server.Interface\bin\Debug\TradeAge.Server.Interface.dll",
                       @"E:\Project\DogSE\TradeAge\Server\TradeAge.Server.Protocol\ServerLogicProtocol.cs");
        }
        const string FileCodeBase = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;
using DogSE.Server.Core.LogicModule;

namespace DogSE.Server.Core.Protocol.AutoCode
{
    /// <summary>
    /// 服务器业务逻辑注册管理器
    /// </summary>
    public static class ServerLogicProtoclRegister
    {
        private static readonly List<IProtoclAutoCode> list = new List<IProtoclAutoCode>();

        /// <summary>
        /// 注册所有模块的网络消息到包管理器里
        /// </summary>
        /// <param name=`modules`></param>
        /// <param name=`handlers`></param>
        public static void Register(ILogicModule[] modules, PacketHandlersBase handlers)
        {
            foreach (var m in modules)
            {
#proxyregister#
            }
        }
    }

#code#
}

";
    }
}
