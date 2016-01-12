using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DogSE.Library.Log;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Tools.CodeGeneration.Utils;

namespace DogSE.Tools.CodeGeneration.Client.Unity3d
{
    /// <summary>
    /// 访问代码创建
    /// </summary>
    class CreateProxyCode
    {
        private readonly Type classType;

        private List<FunItem> funDoc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">要创建的方法的类</param>
        /// <param name="doc"></param>
        public CreateProxyCode(Type type, List<FunItem> doc)
        {
            classType = type;
            funDoc = doc;
        }

        private readonly StringBuilder initCode = new StringBuilder();

        /// <summary>
        /// 调用代码
        /// </summary>
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
        /// <param name="doc">文档注解</param>
        private void AddWriteProxy(Type type, string doc = null)
        {
            if (writerProxySet.Contains(type))
                return;

            writerProxySet.Add(type);

            StringBuilder writeCode = new StringBuilder();
            var typeName = type.Name;

            foreach (var p in type.GetProperties())
            {
                if (!p.CanRead || !p.CanWrite)
                    continue;

                if (p.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                    continue;

                if (p.PropertyType == typeof(int))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(byte))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(long))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(float))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(double))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(bool))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(string))
                {
                    writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof(DateTime))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0}.Ticks);\r\n", p.Name);
                }
                else if (p.PropertyType.IsEnum)
                {
                    writeCode.AppendFormat("pw.Write((byte)obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType.IsLayoutSequential)
                {
                    //writeCode.AppendFormat("pw.WriteStruct(obj.{0});\r\n", p.Name);
                    AddWriteProxyByStruct(p.PropertyType);
                    writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}, pw);\r\n", p.PropertyType.Name, p.Name);
                }
                else if (p.PropertyType.IsArray)
                {
                    //  数组
                    #region 数组的处理

                    var arrayType = p.PropertyType.GetElementType();

                    //  先写入长度
                    writeCode.AppendFormat("pw.Write((int)obj.{0}.Length);\r\n", p.Name);
                    writeCode.AppendFormat("for(int i = 0;i < obj.{0}.Length){{\r\n", p.Name);

                    if (arrayType == typeof(int))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(byte))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(long))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(float))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(double))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(bool))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(string))
                    {
                        writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (p.PropertyType == typeof(DateTime))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}.Ticks);\r\n", p.Name);
                    }
                    else if (arrayType.IsEnum)
                    {
                        writeCode.AppendFormat("pw.Write((byte)obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType.IsLayoutSequential)
                    {
                        //writeCode.AppendFormat("pw.WriteStruct(obj.{0}[i]);\r\n", p.Name);
                        AddWriteProxyByStruct(p.PropertyType);
                        writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", p.PropertyType.Name, p.Name);

                    }

                    writeCode.AppendLine("}");

                    #endregion
                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.PropertyType.Name));
                }

            }

            writerProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", typeName)
                .Replace("#TypeFullName#", Utils.GetFixFullTypeName(type.FullName))
                .Replace("#ReadCode#", writeCode.ToString())
                .Replace("#doc#", doc)
                );
        }

        /// <summary>
        /// 添加一个读取代理类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="doc">文档注解</param>
        private void AddWriteProxyByStruct(Type type, string doc = null)
        {
            if (writerProxySet.Contains(type))
                return;

            writerProxySet.Add(type);

            StringBuilder writeCode = new StringBuilder();
            var typeName = type.Name;

            foreach (var p in type.GetFields())
            {
                if (p.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                    continue;

                if (p.FieldType == typeof(int))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(byte))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(long))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(float))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(double))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(bool))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(string))
                {
                    writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType == typeof(DateTime))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0}.Ticks);\r\n", p.Name);
                }
                else if (p.FieldType.IsEnum)
                {
                    writeCode.AppendFormat("pw.Write((byte)obj.{0});\r\n", p.Name);
                }
                else if (p.FieldType.IsLayoutSequential)
                {
                    //writeCode.AppendFormat("pw.WriteStruct(obj.{0});\r\n", p.Name);
                    AddWriteProxyByStruct(p.FieldType);
                    writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}, pw);\r\n", p.FieldType.Name, p.Name);
                }
                else if (p.FieldType.IsArray)
                {
                    //  数组
                    #region 数组的处理

                    var arrayType = p.FieldType.GetElementType();

                    //  先写入长度
                    writeCode.AppendFormat("pw.Write((int)obj.{0}.Length);\r\n", p.Name);
                    writeCode.AppendFormat("for(int i = 0;i < obj.{0}.Length){{\r\n", p.Name);

                    if (arrayType == typeof(int))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(byte))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(long))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(float))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(double))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(bool))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof(string))
                    {
                        writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (p.FieldType == typeof(DateTime))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}.Ticks);\r\n", p.Name);
                    }
                    else if (arrayType.IsEnum)
                    {
                        writeCode.AppendFormat("pw.Write((byte)obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType.IsLayoutSequential)
                    {
                        //writeCode.AppendFormat("pw.WriteStruct(obj.{0}[i]);\r\n", p.Name);

                        AddWriteProxyByStruct(p.FieldType);
                        writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", p.FieldType.Name, p.Name);
                    }

                    writeCode.AppendLine("}");

                    #endregion
                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.FieldType.Name));
                }

            }

            writerProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", typeName)
                .Replace("#TypeFullName#", Utils.GetFixFullTypeName(type.FullName))
                .Replace("#ReadCode#", writeCode.ToString())
                .Replace("#doc#", doc)
                );
        }
        private string readProxyCodeFormatter = @"

    /// <summary>
    /// #doc#
    /// </summary>
    public class #TypeName#WriteProxy
    {
    /// <summary>
    /// #doc#
    /// </summary>
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
                    streamWriterCode.AppendFormat("var pw = PacketWriter.AcquireContent({0});", att.OpCode);
                    streamWriterCode.AppendLine();
                    streamWriterCode.AppendFormat(
                        @"            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( {0} );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                ", att.OpCode);

                    streamWriterCode.AppendFormat("{0}WriteProxy.Write(obj, pw);", parameterType.Name);

                    streamWriterCode.AppendLine("NetState.Send(pw);");
                    streamWriterCode.AppendLine(" if ( packetProfile != null ) packetProfile.Record(pw.Length);");
                    streamWriterCode.AppendLine("PacketWriter.ReleaseContent(pw);");

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
                    streamWriterCode.AppendFormat("var pw = PacketWriter.AcquireContent({0});", att.OpCode);
                    streamWriterCode.AppendLine();
                    streamWriterCode.AppendFormat(
                        @"            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( {0} );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                ", att.OpCode);

                    streamWriterCode.AppendLine("obj.Write(pw);");

                    streamWriterCode.AppendLine("NetState.Send(pw);");
                    streamWriterCode.AppendLine(" if ( packetProfile != null ) packetProfile.Record(pw.Length);");
                    streamWriterCode.AppendLine("PacketWriter.ReleaseContent(pw);");
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
                StringBuilder commentCode = new StringBuilder();

                Console.WriteLine(classType.FullName + "." + methodinfo.Name);
                var doc =
                    funDoc.FirstOrDefault(o => o.Name.IndexOf("M:" + classType.FullName + "." + methodinfo.Name) == 0);
                if (doc == null)
                    doc = new FunItem();


                commentCode.AppendFormat(@"        /// <summary>
        /// {0}
        /// </summary>
", doc.Summary);

                methonNameCode.AppendFormat("public void {0}(", methodName);

                streamWriterCode.AppendLine("{");
                streamWriterCode.AppendFormat("var pw = PacketWriter.AcquireContent({0});", att.OpCode);
                streamWriterCode.AppendLine();
                
                for (int i = 1; i < param.Length; i++)
                {
                    var p = param[i];
                    if (p.ParameterType == typeof(int))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("int {0},", p.Name);                        
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(byte))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("byte {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(long))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("long {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(float))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("float {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(double))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("double {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(bool))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("bool {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(string))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("string {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.WriteUTF8Null({0});\r\n", p.Name);
                    }
                    else if (p.ParameterType == typeof(DateTime))
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("DateTime {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0}.Ticks);\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsEnum)
                    {
                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                        streamWriterCode.AppendFormat("pw.Write((byte){0});\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsLayoutSequential)
                    {
                        AddWriteProxyByStruct(p.ParameterType, doc.GetParamSummary(p.Name));

                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                        //streamWriterCode.AppendFormat("pw.WriteStruct({0});\r\n", p.Name);
                        streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}, pw);\r\n", p.ParameterType.Name, p.Name);
                    }
                    else if (p.ParameterType.IsArray)
                    {
                        #region 数组的处理

                        var arrayType = p.ParameterType.GetElementType();

                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);

                        //  先写入长度
                        streamWriterCode.AppendFormat("pw.Write((int){0}.Length);\r\n", p.Name);
                        streamWriterCode.AppendFormat("for(int i = 0;i < {0}.Length;i++){{\r\n", p.Name);

                        if (arrayType == typeof(int))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType == typeof(byte))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType == typeof(long))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType == typeof(float))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType == typeof(double))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType == typeof(bool))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType == typeof(string))
                        {
                            streamWriterCode.AppendFormat("pw.WriteUTF8Null({0}[i]);\r\n", p.Name);
                        }
                        else if (p.ParameterType == typeof(DateTime))
                        {
                            streamWriterCode.AppendFormat("pw.Write({0}.Ticks);\r\n", p.Name);
                        }
                        else if (arrayType.IsEnum)
                        {
                            streamWriterCode.AppendFormat("pw.Write((byte){0}[i]);\r\n", p.Name);
                        }
                        else if (arrayType.IsLayoutSequential)
                        {
                            //streamWriterCode.AppendFormat("pw.WriteStruct({0}[i]);\r\n", p.Name);

                            AddWriteProxyByStruct(p.ParameterType);
                            streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}[i], pw);\r\n", p.ParameterType.Name, p.Name);
                        }

                        streamWriterCode.AppendLine("}");

                        #endregion
                    }
                    else if (p.ParameterType.IsClass)
                    {
                        AddWriteProxy(p.ParameterType, doc.GetParamSummary(p.Name));

                        commentCode.AppendFormat("/// <param name=`{0}`>{1}</param>\r\n", p.Name, doc.GetParamSummary(p.Name));
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                        streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}, pw);\r\n", p.ParameterType.Name, p.Name);
                    }
                    else
                    {
                        Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                            classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                    }

                }

                streamWriterCode.AppendLine("NetState.Send(pw);PacketWriter.ReleaseContent(pw);");
                streamWriterCode.AppendLine("}");

                if (param.Length > 1)
                    methonNameCode.Remove(methonNameCode.Length - 1, 1);

                methonNameCode.Append(")");

                callCode.AppendLine(commentCode.ToString());
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
            //Console.WriteLine(code);
            return code;
        }

        /// <summary>
        /// 编译后生成的组件
        /// </summary>
        public Assembly CompiledAssembly { get; set; }

        private const string CodeBase = @"

    /// <summary>
    /// 
    /// </summary>
    partial class #ClassName#Controller
    {
        #CallMethod#

#ProxyCode#
    }

";
    }



    /// <summary>
    /// 客户端调用服务器的代理代码生成
    /// </summary>
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

            var xmlFile = dllFile.Replace(".dll", ".xml");
            var xmlDoc = CodeCommentUtils.LoadXmlDocument(xmlFile);

            foreach (var type in dll.GetTypes())
            {
                if (type.IsInterface)
                {
                    var i = type.GetInterface("DogSE.Server.Core.LogicModule.ILogicModule");
                    if (i != null)
                    {
                        Console.WriteLine(type.ToString());

                        var crc = new CreateProxyCode(type, xmlDoc);
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
