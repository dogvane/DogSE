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
using System.Linq;
using DogSE.Tools.CodeGeneration.Client.Unity3d;

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

            writerProxySet.Add(type);

            StringBuilder writeCode = new StringBuilder();

            foreach (var p in type.GetProperties())
            {
                if (!p.CanRead || !p.CanWrite)
                    continue;

                if (p.GetCustomAttributes(typeof (IgnoreAttribute), true).Length > 0)
                    continue;

                if (p.PropertyType == typeof (int))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (byte))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (long))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (float))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (double))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (bool))
                {
                    writeCode.AppendFormat("pw.Write(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (string))
                {
                    writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0});\r\n", p.Name);
                }
                else if (p.PropertyType == typeof (DateTime))
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
                    writeCode.AppendFormat("for(int i = 0;i < obj.{0}.Length;i++){{\r\n", p.Name);

                    if (arrayType == typeof (int))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof (byte))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof (long))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof (float))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof (double))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof (bool))
                    {
                        writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType == typeof (string))
                    {
                        writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType.IsEnum)
                    {
                        writeCode.AppendFormat("pw.Write((byte)obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType.IsLayoutSequential)
                    {
                        //writeCode.AppendFormat("pw.WriteStruct(obj.{0}[i]);\r\n", p.Name);
                        AddWriteProxyByStruct(arrayType);

                        writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", arrayType.Name, p.Name);
                    }
                    else if (arrayType.IsClass)
                    {
                        AddWriteProxy(arrayType);

                        writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", arrayType.Name, p.Name);
                    }

                    writeCode.AppendLine("}");

                    #endregion
                }
                else if (p.PropertyType.IsGenericType)
                {
                    if (p.PropertyType.Name.Contains("List"))
                    {
                        // 列表

                        #region 数组的处理

                        var gType = p.PropertyType.GetGenericArguments()[0];

                        //  先写入长度
                        writeCode.AppendFormat("pw.Write((int)obj.{0}.Count);\r\n", p.Name);
                        writeCode.AppendFormat("for(int i = 0;i < obj.{0}.Count;i++){{\r\n", p.Name);

                        if (gType == typeof (int))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof (byte))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof (long))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof (float))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof (double))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof (bool))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof (string))
                        {
                            writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType.IsEnum)
                        {
                            writeCode.AppendFormat("pw.Write((byte)obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType.IsLayoutSequential)
                        {
                            //writeCode.AppendFormat("pw.WriteStruct(obj.{0}[i]);\r\n", p.Name);
                            AddWriteProxyByStruct(gType);

                            writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", gType.Name, p.Name);
                        }
                        else if (gType.IsClass)
                        {
                            AddWriteProxy(gType);

                            writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", gType.Name, p.Name);
                        }

                        writeCode.AppendLine("}");

                        #endregion
                    }
                }
                else if (p.PropertyType.IsClass)
                {
                    AddWriteProxy(p.PropertyType);

                    writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}, pw);\r\n", p.PropertyType.Name, p.Name);
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
                .Replace("#ReadCode#", writeCode.ToString())
                );
        }

        /// <summary>
        /// 添加一个读取代理类
        /// </summary>
        /// <param name="type"></param>
        private void AddWriteProxyByStruct(Type type)
        {
            if (writerProxySet.Contains(type))
                return;

            writerProxySet.Add(type);

            StringBuilder writeCode = new StringBuilder();

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
                    writeCode.AppendFormat("for(int i = 0;i < obj.{0}.Length;i++){{\r\n", p.Name);

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
                    else if (arrayType.IsEnum)
                    {
                        writeCode.AppendFormat("pw.Write((byte)obj.{0}[i]);\r\n", p.Name);
                    }
                    else if (arrayType.IsLayoutSequential)
                    {
                        //writeCode.AppendFormat("pw.WriteStruct(obj.{0}[i]);\r\n", p.Name);
                        AddWriteProxyByStruct(arrayType);

                        writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", arrayType.Name, p.Name);
                    }
                    else if (arrayType.IsClass)
                    {
                        AddWriteProxy(arrayType);

                        writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", arrayType.Name, p.Name);
                    }

                    writeCode.AppendLine("}");

                    #endregion
                }
                else if (p.FieldType.IsGenericType)
                {
                    if (p.FieldType.Name.Contains("List"))
                    {
                        // 列表

                        #region 数组的处理

                        var gType = p.FieldType.GetGenericArguments()[0];

                        //  先写入长度
                        writeCode.AppendFormat("pw.Write((int)obj.{0}.Count);\r\n", p.Name);
                        writeCode.AppendFormat("for(int i = 0;i < obj.{0}.Count;i++){{\r\n", p.Name);

                        if (gType == typeof(int))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof(byte))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof(long))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof(float))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof(double))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof(bool))
                        {
                            writeCode.AppendFormat("pw.Write(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType == typeof(string))
                        {
                            writeCode.AppendFormat("pw.WriteUTF8Null(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType.IsEnum)
                        {
                            writeCode.AppendFormat("pw.Write((byte)obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType.IsLayoutSequential)
                        {
                            AddWriteProxyByStruct(gType);

                            writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", gType.Name, p.Name);

                            //writeCode.AppendFormat("pw.WriteStruct(obj.{0}[i]);\r\n", p.Name);
                        }
                        else if (gType.IsClass)
                        {
                            AddWriteProxy(gType);

                            writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}[i], pw);\r\n", gType.Name, p.Name);
                        }

                        writeCode.AppendLine("}");

                        #endregion
                    }
                }
                else if (p.FieldType.IsClass)
                {
                    AddWriteProxy(p.FieldType);

                    writeCode.AppendFormat("{0}WriteProxy.Write(obj.{1}, pw);\r\n", p.FieldType.Name, p.Name);
                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.FieldType.Name));
                }

            }

            writerProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", type.Name)
                .Replace("#TypeFullName#", type.FullName)
                .Replace("#ReadCode#", writeCode.ToString())
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
                    methonNameCode.AppendFormat("public void {0}(NetState netstate, {1} obj)",
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

                    streamWriterCode.AppendLine("netState.Send(pw);");
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
                    methonNameCode.AppendFormat("public void {0}(NetState netstate, {1} obj)", 
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

                    streamWriterCode.AppendLine("netstate.Send(pw);");
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
                

                string methodName = methodinfo.Name;
                StringBuilder methonNameCode = new StringBuilder();
                StringBuilder streamWriterCode = new StringBuilder();
                methonNameCode.AppendFormat("public void {0}(NetState netstate,", methodName);

                streamWriterCode.AppendLine("{");
                streamWriterCode.AppendFormat("var pw = PacketWriter.AcquireContent({0});", att.OpCode);
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
                    else if (p.ParameterType == typeof(DateTime))
                    {
                        methonNameCode.AppendFormat("DateTime {0},", p.Name);
                        streamWriterCode.AppendFormat("pw.Write({0}.Ticks);\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsEnum)
                    {
                        methonNameCode.AppendFormat("{0} {1},", p.ParameterType.FullName, p.Name);
                        streamWriterCode.AppendFormat("pw.Write((byte){0});\r\n", p.Name);
                    }
                    else if (p.ParameterType.IsLayoutSequential)
                    {
                        methonNameCode.AppendFormat("{0} {1},", p.ParameterType.FullName, p.Name);
                        AddWriteProxyByStruct(p.ParameterType);
                        //streamWriterCode.AppendFormat("pw.WriteStruct({0});\r\n", p.Name);
                        streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}, pw);\r\n", p.ParameterType.Name, p.Name);
                    }
                        else if (p.ParameterType.IsArray)
                        {
                            #region 数组的处理

                            var arrayType = p.ParameterType.GetElementType();

                            methonNameCode.AppendFormat("{0} {1},", p.ParameterType.FullName, p.Name);

                            streamWriterCode.AppendFormat("int {0}len = {0} == null ? 0:{0}.Length;", p.Name);

                            //  先写入长度
                            streamWriterCode.AppendFormat("pw.Write({0}len);\r\n", p.Name);
                            streamWriterCode.AppendFormat("for(int i = 0;i < {0}len ;i++){{\r\n", p.Name);

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
                            else if (arrayType.IsEnum)
                            {
                                streamWriterCode.AppendFormat("pw.Write((byte){0}[i]);\r\n", p.Name);
                            }
                            else if (arrayType.IsLayoutSequential)
                            {
                            //streamWriterCode.AppendFormat("pw.WriteStruct({0}[i]);\r\n", p.Name);
                            AddWriteProxyByStruct(arrayType);
                            streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}[i], pw);\r\n", arrayType.Name, p.Name);
                        }
                            else if (arrayType.IsClass)
                            {
                                AddWriteProxy(arrayType);
                                streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}[i], pw);\r\n", arrayType.Name, p.Name);
                            }

                            streamWriterCode.AppendLine("}");

                            #endregion
                        }
                    else if (p.ParameterType.IsClass)
                    {
                        AddWriteProxy(p.ParameterType);

                        methonNameCode.AppendFormat("{0} {1},", p.ParameterType.FullName, p.Name);
                        streamWriterCode.AppendFormat("{0}WriteProxy.Write({1}, pw);\r\n", p.ParameterType.Name, p.Name);
                    }
                    else
                    {
                        Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                            classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                    }

                }

                streamWriterCode.AppendLine("netstate.Send(pw);");
                streamWriterCode.AppendLine(" if ( packetProfile != null ) packetProfile.Record(pw.Length);");
                streamWriterCode.AppendLine("PacketWriter.ReleaseContent(pw);");

                
                streamWriterCode.AppendLine("}");

                methonNameCode.Remove(methonNameCode.Length - 1, 1);
                methonNameCode.Append(")");

                callCode.AppendLine(methonNameCode.ToString());
                callCode.AppendLine(streamWriterCode.ToString());

                #endregion
            }
        }

        private static int s_version = 1;

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
            ret.Replace("#version#", s_version.ToString());
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
    class #ClassName#Proxy#version#:#FullClassName#
    {
        #CallMethod#

#ProxyCode#
    }

";
    }
    

    /// <summary>
    /// 服务器上的客户端代理代码创建
    /// </summary>
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
