using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DogSE.Library.Log;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;

namespace DogSE.Tools.CodeGeneration.Client.Unity3d
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
        /// 读取代理类
        /// </summary>
        private readonly StringBuilder writerProxyCode = new StringBuilder();


        private HashSet<Type> writerProxySet = new HashSet<Type>();

        /// <summary>
        /// 添加一个读取代理类
        /// </summary>
        /// <param name="type"></param>
        private void AddWriteProxy(Type type)
        {
            if (writerProxySet.Contains(type))
                return;

            StringBuilder readCode = new StringBuilder();

            foreach (var p in type.GetProperties())
            {
                if (p.PropertyType == typeof(int))
                {
                    readCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(long))
                {
                    readCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(float))
                {
                    readCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(double))
                {
                    readCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(bool))
                {
                    readCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(string))
                {
                    readCode.AppendFormat("pw.WriteUTF8Null(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType.IsEnum)
                {
                    readCode.AppendFormat("pw.Write((byte)obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType.IsLayoutSequential)
                {
                    readCode.AppendFormat("pw.WriteStruct(obj.{0});\r\n", p.Name);

                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.PropertyType.Name));
                }

            }

            writerProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", type.Name)
                .Replace("#TypeFullName#", type.FullName)
                .Replace("#ReadCode#", readCode.ToString())
                );
        }

        private string readProxyCodeFormatter = @"
    public class #TypeName#WriteProxy
    {
        public static void Write(#TypeFullName# obj, PacketWriter pw)
        {

#ReadCode#
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
                #region ProtocolStruct

                Type parameterType = param[1].ParameterType;

                if (!parameterType.IsClass)
                {
                    Logs.Error("{0}.{1} 的第二个参数必须是class类型。", classType.Name, methodinfo.Name);
                    return;
                }

                if (parameterType.GetInterface(typeof(IPacketWriter).FullName) == null)
                {
                    //  自己实现一个对对象的协议写入类
                    AddWriteProxy(parameterType);

                    string methodName = methodinfo.Name;
                    StringBuilder methonNameCode = new StringBuilder();
                    StringBuilder streamWriterCode = new StringBuilder();
                    methonNameCode.AppendFormat("public void {0}({1} obj)",
                        methodName, parameterType.FullName);

                    streamWriterCode.AppendLine("{");
                    streamWriterCode.AppendFormat("var pw = new PacketWriter({0});", att.OpCode);
                    streamWriterCode.AppendLine();
                    streamWriterCode.AppendFormat(
                        @"            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( {0} );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                ", att.OpCode);

                    streamWriterCode.AppendFormat("{0}WriteProxy.Write(obj, pw);", parameterType.Name);

                    streamWriterCode.AppendLine("NetState.Send(pw);pw.Dispose();");
                    streamWriterCode.AppendLine("}");

                    methonNameCode.Remove(methonNameCode.Length - 1, 1);
                    methonNameCode.Append(")");

                    callCode.AppendLine(methonNameCode.ToString());
                    callCode.AppendLine(streamWriterCode.ToString());
                }
                else
                {
                    //  如果对象实现了 IPacketWriter 接口，则直接使用，否则则自己生成协议代码
                    string methodName = methodinfo.Name;
                    StringBuilder methonNameCode = new StringBuilder();
                    StringBuilder streamWriterCode = new StringBuilder();
                    methonNameCode.AppendFormat("public void {0}({1} obj)",
                        methodName, parameterType.FullName);

                    streamWriterCode.AppendLine("{");
                    streamWriterCode.AppendFormat("var pw = new PacketWriter({0});", att.OpCode);
                    streamWriterCode.AppendLine();
                    streamWriterCode.AppendFormat(
                        @"            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( {0} );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                ", att.OpCode);

                    streamWriterCode.AppendLine("obj.Write(pw);");

                    streamWriterCode.AppendLine("NetState.Send(pw);pw.Dispose();");
                    streamWriterCode.AppendLine("}");

                    methonNameCode.Remove(methonNameCode.Length - 1, 1);
                    methonNameCode.Append(")");

                    callCode.AppendLine(methonNameCode.ToString());
                    callCode.AppendLine(streamWriterCode.ToString());
                }

                #endregion
            }

            if (att.MethodType == NetMethodType.SimpleMethod)
            {
                #region SimpleMethod


                string methodName = Utils.GetFixCallProxyName(methodinfo.Name);

                StringBuilder methonNameCode = new StringBuilder();
                StringBuilder streamWriterCode = new StringBuilder();
                methonNameCode.AppendFormat("public void {0}(", methodName);

                streamWriterCode.AppendLine("{");
                streamWriterCode.AppendFormat("var pw = new PacketWriter({0});", att.OpCode);
                streamWriterCode.AppendLine();


                for (int i = 1; i < param.Length; i++)
                {
                    var p = param[i];
                    if (p.ParameterType == typeof(int))
                    {
                        methonNameCode.AppendFormat("int {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(long))
                    {
                        methonNameCode.AppendFormat("long {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(float))
                    {
                        methonNameCode.AppendFormat("float {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(double))
                    {
                        methonNameCode.AppendFormat("double {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(bool))
                    {
                        methonNameCode.AppendFormat("bool {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(string))
                    {
                        methonNameCode.AppendFormat("string {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.WriteUTF8Null({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsEnum)
                    {
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                        streamWriterCode.AppendFormat("pw.Write((byte){0});\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsLayoutSequential)
                    {
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                        streamWriterCode.AppendFormat("pw.WriteStruct({0});\r\n", p.Name);

                    }
                    else
                    {
                        Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                            classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                    }

                }

                streamWriterCode.AppendLine("NetState.Send(pw);");
                streamWriterCode.AppendLine("}");

                methonNameCode.Remove(methonNameCode.Length - 1, 1);
                methonNameCode.Append(")");

                callCode.AppendLine(methonNameCode.ToString());
                callCode.AppendLine(streamWriterCode.ToString());

                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetCode()
        {
            var ret = new StringBuilder();

            ret.Append(CodeBase);
            ret.Replace("#ClassName#",  Utils.GetFixInterfaceName(classType.Name));
            ret.Replace("#FullClassName#", classType.FullName);
            ret.Replace("#InitMethod#", initCode.ToString());
            ret.Replace("#CallMethod#", callCode.ToString());
            ret.Replace("#ProxyCode#", writerProxyCode.ToString());

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

    partial class #ClassName#Controller
    {
        #CallMethod#

#ProxyCode#
    }

";
    }



    class ServerProxyProtocolGeneration
    {
        /// <summary>
        /// 创建代码
        /// </summary>
        /// <param name="dllFile"></param>
        /// <param name="outFolder"></param>
        /// <param name="nameSpace"></param>
        public static void CreateCode(string dllFile, string outFolder, string nameSpace)
        {
            StringBuilder codeBuilder = new StringBuilder();

            var dll = Assembly.LoadFrom(dllFile);

            foreach (var type in dll.GetTypes())
            {
                if (type.IsInterface)
                {
                    var i = type.GetInterface("DogSE.Server.Core.LogicModule.ILogicModule");
                    if (i != null)
                    {
                        Console.WriteLine(type.ToString());

                        var crc = new CreateProxyCode(type);
                        var code = crc.CreateCode();

                        string typeName = Utils.GetFixInterfaceName(type.Name);

                        string fileName = Path.Combine(outFolder, string.Format(@"Controller\{0}\{0}Controller.Proxy.cs", typeName));
                        var fi = new FileInfo(fileName);
                        if (!fi.Directory.Exists)
                            fi.Directory.Create();

                        var fileContext = FileCodeBase
    .Replace("#code#", code)
    .Replace("#namepace#", nameSpace)
    .Replace("#TypeName#", typeName)
    .Replace("`", "\"");

                        File.WriteAllText(fileName, fileContext, Encoding.UTF8);
                    }
                }
            }



            
        }


        const string FileCodeBase = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace #namepace#.Controller.#TypeName#
{
#code#
}

";
    }

}
