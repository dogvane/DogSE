using System;
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
        /// 协议包读取
        /// </summary>
        public interface IPacketReader
        {
            /// <summary>
            /// 数据读取
            /// </summary>
            /// <param name="reader"></param>
            void Read(PacketReader reader);
        }


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
            /// 添加一个方法
            /// </summary>
            /// <param name="att"></param>
            /// <param name="methodinfo"></param>
            void AddMethod(NetMethodAttribute att, MethodInfo methodinfo)
            {
                var param = methodinfo.GetParameters();
                if (param.Length < 2)
                {
                    Logs.Error(string.Format("{0}.{1} 不支持 {2} 个参数", classType.Name, methodinfo.Name, param.Length.ToString()));
                    return;
                }

                if (param[0].ParameterType != typeof(NetState))
                {
                    Logs.Error("{0}.{1} 的第一个参数必须是 NetState 对象", classType.Name, methodinfo.Name);
                    return;
                }

                if (att.MethodType == NetMethodType.PacketReader)
                {
                    if (param[1].ParameterType != typeof(PacketReader))
                    {
                        Logs.Error("{0}.{1} 的第二个参数必须是 PacketReader 对象", classType.Name, methodinfo.Name);
                        return;
                    }

                    //string mehtodName = methodinfo.Name;
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
                    if (param[1].ParameterType.GetInterface(typeof(IPacketReader).FullName) == null)
                    {
                        Logs.Error("{0}.{1} 的第二个参数必须实现 IPacketReader 接口", classType.Name, methodinfo.Name);
                        return;
                    }

                    if (!param[1].ParameterType.IsClass)
                    {
                        Logs.Error("{0}.{1} 的第二个参数必须是class类型。", classType.Name, methodinfo.Name);
                        return;
                    }

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

                if (att.MethodType == NetMethodType.SimpleMethod)
                {
                    string methodName = methodinfo.Name;
                    initCode.AppendFormat("PacketHandlerManager.Register({0}, {1});",
                                          att.OpCode, methodName);
                    initCode.AppendLine();

                    callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", methodName);
                    callCode.AppendLine("{");
                    for (int i = 1; i < param.Length; i++)
                    {
                        var p = param[i];
                        if (p.ParameterType == typeof(int))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadInt32();\r\n", i);
                        }
                        else if (p.ParameterType == typeof(long))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadLong64();\r\n", i);
                        }
                        else if (p.ParameterType == typeof(float))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadFloat();\r\n", i);
                        }
                        else if (p.ParameterType == typeof(double))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadFloat();\r\n", i);
                        }
                        else if (p.ParameterType == typeof(bool))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadBoolean();\r\n", i);
                        }
                        else if (p.ParameterType == typeof(string))
                        {
                            callCode.AppendFormat("var p{0} = reader.ReadUTF8String();\r\n", i);
                        }
                        else
                        {
                            Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                                                     classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                        }

                    }

                    callCode.AppendFormat("module.{0}(netstate", methodinfo.Name);
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
                Console.WriteLine(code);
                return code;
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
    }
";
        }
    


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


            foreach(var type in dll.GetTypes())
            {
                if (type.IsInterface)
                {
                    var i = type.GetInterface("DogSE.Server.Core.LogicModule.ILogicModule");
                    if (i != null)
                    {
                        var code = new CreateReadCode(type).CreateCode();
                        codeBuilder.Append(code);
                    }
                }
            }

            var fileContext = FileCodeBase.Replace("#code#", codeBuilder.ToString());
            File.WriteAllText(outFile, fileContext);
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
#code#
}

";
    }
}
