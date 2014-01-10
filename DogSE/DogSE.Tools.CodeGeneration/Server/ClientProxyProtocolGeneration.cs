using System;
using System.IO;
using System.Reflection;
using System.Text;
using DogSE.Library.Log;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Task;
using System.Linq;

namespace DogSE.Tools.CodeGeneration.Server
{

    /// <summary>
    /// 访问代码创建
    /// </summary>
    class CreateProxyCode
    {
        private readonly Type classType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">要创建的方法的类</param>
        public CreateProxyCode(Type type)
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
            if (param.Length < 1)
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
                Logs.Error("客户端代理类不支持这种模式 {0}", att.MethodType.ToString());
                return;
            }

            if (att.MethodType == NetMethodType.ProtocolStruct)
            {
                //  TODO: 稍后补充这种模式
                Logs.Error("客户端代理类暂时不支持这种模式 {0}", att.MethodType.ToString());
                return;
            }

            if (att.MethodType == NetMethodType.SimpleMethod)
            {
                string methodName = methodinfo.Name;
                StringBuilder methonNameCode = new StringBuilder();
                StringBuilder streamWriterCode = new StringBuilder();
                methonNameCode.AppendFormat("public void {0}(NetState netstate,", methodName);

                streamWriterCode.AppendLine("{");
                streamWriterCode.AppendFormat("var pw = new PacketWriter({0});", att.OpCode);
                streamWriterCode.AppendLine();
                streamWriterCode.AppendFormat(
                    @"            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( {0} );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                ", att.OpCode);


                for (int i = 1; i < param.Length; i++)
                {
                    var p = param[i];
                    if (p.ParameterType == typeof (int))
                    {
                        methonNameCode.AppendFormat("int {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof (long))
                    {
                        methonNameCode.AppendFormat("long {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof (float))
                    {
                        methonNameCode.AppendFormat("float {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof (double))
                    {
                        methonNameCode.AppendFormat("double {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof (bool))
                    {
                        methonNameCode.AppendFormat("bool {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof (string))
                    {
                        methonNameCode.AppendFormat("string {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.WriteUTF8Null({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsEnum)
                    {
                        methonNameCode.AppendFormat("{0} {1},", p.ParameterType.FullName, p.Name);
                        streamWriterCode.AppendFormat("pw.Write((byte){0});\r\n", p.Name);
                    }
                    else
                    {
                        Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                            classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                    }

                }

                streamWriterCode.AppendLine("netstate.Send(pw);pw.Dispose();");
                streamWriterCode.AppendLine("}");

                methonNameCode.Remove(methonNameCode.Length - 1, 1);
                methonNameCode.Append(")");

                callCode.AppendLine(methonNameCode.ToString());
                callCode.AppendLine(streamWriterCode.ToString());
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

            var nsName = classType.Namespace;

            ret.Replace("#using#", string.Format("using {0};", nsName));
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
    class #ClassName#Proxy#version#:#FullClassName#
    {
        #CallMethod#
    }

";
    }
    


    class ClientProxyProtocolGeneration
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

            Type clientProxyType = null;
            foreach(var type in dll.GetTypes())
            {
                if (type.Name == "ClientProxy")
                {
                    clientProxyType = type;
                }

                if (type.IsInterface)
                {
                    var i = type.GetCustomAttributes(typeof (ClientInterfaceAttribute), true);
                    if ( i.Length > 0)
                    {
                        var code = new CreateProxyCode(type).CreateCode();
                        codeBuilder.Append(code);
                    }
                }
            }

            StringBuilder registerCode = new StringBuilder();
            if (clientProxyType != null)
            {
                foreach (var p in clientProxyType.GetProperties())
                {
                    registerCode.AppendFormat("{0}.{1} = new {2}Proxy1();\r\n", clientProxyType.FullName, p.Name, p.PropertyType.Name);                    
                }
            }

            var fileContext = FileCodeBase
                .Replace("#code#", codeBuilder.ToString())
                .Replace("#proxyregister#", registerCode.ToString());
                //.Replace("#using#", string.Format("using {0};", clientProxyType.FullName));

            File.WriteAllText(outFile, fileContext, Encoding.UTF8);
        }


        void test()
        {
            CreateCode(@"E:\Project\DogSE\TradeAge\TradeAge.Server.Interface\bin\Debug\TradeAge.Server.Interface.dll",
                       @"E:\Project\DogSE\TradeAge\Server\TradeAge.Server.Protocol\ClientProxyProtocol.cs");
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
    /// 代理客户端注册
    /// </summary>
    public static class ClientProxyRegister
    {
        /// <summary>
        /// 注册代理类
        /// </summary>
        public static void Register()
        {
#proxyregister#
        }
    }
#code#
}

";
    }
}
