using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using DogSE.Library.Log;
using DogSE.Server.Core.Net;
using Microsoft.CSharp;

namespace DogSE.Server.Core.Protocol
{
    /// <summary>
    /// 创建客户端注册代码
    /// </summary>
    public static class CreateClientProxyCode
    {
        /// <summary>
        /// 注册一个静态的对象接口
        /// </summary>
        /// <param name="staticProxyType"></param>
        public static void Register(Type staticProxyType)
        {
            if (!(staticProxyType.IsClass && staticProxyType.IsSealed))
            {
                Logs.Error("类型 {0} 需要时静态类型", staticProxyType.Name);
                return;
            }

            var propertyTypes =
                staticProxyType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (var type in propertyTypes)
            {
                var cpc = new CreateProxyCode(type.PropertyType);
                var obj = cpc.CreateCodeAndBuilder();

                type.SetValue(null, obj, null);
            }
        }
    }

    /*
    /// <summary>
    /// 客户端代理，所有给客户端的数据都从这里发送
    /// </summary>
    public static class ClientProxy
    {
        /// <summary>
        /// 客户端时间
        /// </summary>
        public static IGetPingTimeLogicBack GetPingTime { get; set; }


        private static void test()
        {
            CreateClientProxyCode.Register(typeof(ClientProxy));

            Console.WriteLine(GetPingTime == null);
        }
    }

    /// <summary>
    /// 客户端接口
    /// </summary>
    [ClientInterface]
    public interface IGetPingTimeLogicBack
    {
        /// <summary>
        /// 获取时间返回
        /// </summary>
        /// <param name="net"></param>
        /// <param name="serverTime"></param>
        [NetMethod(2, NetMethodType.SimpleMethod)]
        void GetTimeResult(NetState net, long serverTime);

        /// <summary>
        /// 获取时间返回
        /// </summary>
        /// <param name="net"></param>
        [NetMethod(3, NetMethodType.SimpleMethod)]
        void GetTimeResult2(NetState net);
    }
    */


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
        public object CreateCodeAndBuilder()
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
            return Builder(code);
        }

        object Builder(string code)
        {
            using (var csharpCodeProvider = new CSharpCodeProvider())
            {
                var compilerParameters = new CompilerParameters();

                compilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("DogSE.Library.dll");
                compilerParameters.ReferencedAssemblies.Add("DogSE.Server.Core.dll");

                var assFileFullName = classType.Assembly.CodeBase;
                var rightLen = assFileFullName.LastIndexOf("/");
                var assFile = classType.Assembly.CodeBase.Substring(rightLen + 1, assFileFullName.Length - rightLen - 1);
                compilerParameters.ReferencedAssemblies.Add(assFile);

                compilerParameters.WarningLevel = 4;
                compilerParameters.GenerateInMemory = true;

                CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);

                if (compilerResults.Errors.Count > 0)
                {
                    Logs.Error("在动态编译游戏客户端代理类 {0} 时失败。", "");
                    foreach (var error in compilerResults.Errors)
                    {
                        Console.WriteLine(error.ToString());
                        Logs.Error(error.ToString());
                    }
                    return null;
                }

                string className = string.Format("DogSE.Server.Core.Protocol.AutoCode.{0}Proxy{1}", classType.Name, Version);

                var obj = compilerResults.CompiledAssembly.CreateInstance(className);

                if (obj == null)
                {
                    Logs.Error("在创建动态客户端代理类 {0} 时失败。", classType.Name);
                    return null;
                }

                CompiledAssembly = compilerResults.CompiledAssembly;

                return obj;
            }
        }

        /// <summary>
        /// 编译后生成的组件
        /// </summary>
        public Assembly CompiledAssembly { get; set; }

        private const string CodeBase = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;
#using#

namespace DogSE.Server.Core.Protocol.AutoCode
{
    class #ClassName#Proxy#version#:#ClassName#
    {
        #CallMethod#
    }
}

";
    }
}
