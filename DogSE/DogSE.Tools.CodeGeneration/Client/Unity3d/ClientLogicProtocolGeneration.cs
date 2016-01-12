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
            int i = 0;
            foreach (var p in type.GetProperties())
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
                else if (p.PropertyType == typeof (DateTime))
                {
                    readCode.AppendFormat("ret.{0} = new DateTime(reader.ReadLong64());\r\n", p.Name);
                }
                else if (p.PropertyType.IsEnum)
                {
                    readCode.AppendFormat("ret.{0} = ({1})reader.ReadByte();\r\n", p.Name, Utils.GetFixFullTypeName(p.PropertyType.FullName));
                }
                else if (p.PropertyType.IsLayoutSequential)
                {
                    //readCode.AppendFormat("ret.{0} = reader.ReadStruct <{1}>();\r\n", p.Name, Utils.GetFixFullTypeName(p.PropertyType.FullName));
                    AddReadProxyByStruct(p.PropertyType);
                    readCode.AppendFormat(" ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.PropertyType.Name);
                }
                else if (p.PropertyType.IsArray)
                {
                    #region 处理数组的读取

                    i++;

                    var arrayType = p.PropertyType.GetElementType();

                    readCode.AppendFormat("var len{0} = reader.ReadInt32();\r\n", i);
                    readCode.AppendFormat("var p{0} = new {1}[len{0}];", i, Utils.GetFixFullTypeName(arrayType.FullName));   //  这里只创建值类型

                    readCode.AppendFormat("for(int i =0;i< len{0};i++){{\r\n", i);
                    if (arrayType == typeof(int))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadInt32();\r\n", i);
                    }
                    else if (arrayType == typeof(byte))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadByte();\r\n", i);
                    }
                    else if (arrayType == typeof(long))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadLong64();\r\n", i);
                    }
                    else if (arrayType == typeof(float))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadFloat();\r\n", i);
                    }
                    else if (arrayType == typeof(double))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadFloat();\r\n", i);
                    }
                    else if (arrayType == typeof(bool))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadBoolean();\r\n", i);
                    }
                    else if (arrayType == typeof(string))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadUTF8String();\r\n", i);
                    }
                    else if (arrayType.IsEnum)
                    {
                        readCode.AppendFormat("p{0}[i] = ({1})reader.ReadByte();\r\n", i, Utils.GetFixFullTypeName(p.PropertyType.GetElementType().FullName));
                    }
                    else if (arrayType.IsLayoutSequential)
                    {
                        //readCode.AppendFormat("p{0}[i] = reader.ReadStruct <{1}>();\r\n", i, Utils.GetFixFullTypeName(p.PropertyType.FullName));
                        AddReadProxyByStruct(arrayType);
                        readCode.AppendFormat("p{1}[i] = {0}ReadProxy.Read(reader);\r\n", arrayType.Name, i);
                    }
                    else if (arrayType.IsClass)
                    {
                        AddReadProxy(arrayType);
                        readCode.AppendFormat("p{1}[i] = {0}ReadProxy.Read(reader);\r\n", arrayType.Name, i);
                    }

                    readCode.AppendLine("}");

                    readCode.AppendFormat("ret.{0} = p{1};\r\n", p.Name, i);

                    #endregion
                }
                else if (p.PropertyType.IsGenericType)
                {
                    if (p.PropertyType.Name.IndexOf("List") > -1)
                    {
                        //  泛型的列表
                        var gType = p.PropertyType.GetGenericArguments()[0];
                        readCode.AppendFormat("ret.{0} = new System.Collections.Generic.List<{1}>();\r\n", p.Name, Utils.GetFixFullTypeName(gType.FullName));
                        readCode.AppendFormat("var len{0} = reader.ReadInt32();;\r\n", i);
                        readCode.AppendFormat("for(int i =0;i< len{0};i++){{\r\n", i);
                        if (gType == typeof(int))
                        {
                            readCode.AppendFormat("var newData = reader.ReadInt32();\r\n", i);
                        }
                        else if (gType == typeof(byte))
                        {
                            readCode.AppendFormat("var newData = reader.ReadByte();\r\n", i);
                        }
                        else if (gType == typeof(long))
                        {
                            readCode.AppendFormat("var newData = reader.ReadLong64();\r\n", i);
                        }
                        else if (gType == typeof(float))
                        {
                            readCode.AppendFormat("var newData = reader.ReadFloat();\r\n", i);
                        }
                        else if (gType == typeof(double))
                        {
                            readCode.AppendFormat("var newData = reader.ReadFloat();\r\n", i);
                        }
                        else if (gType == typeof(bool))
                        {
                            readCode.AppendFormat("var newData = reader.ReadBoolean();\r\n", i);
                        }
                        else if (gType == typeof(string))
                        {
                            readCode.AppendFormat("var newData = reader.ReadUTF8String();\r\n", i);
                        }
                        else if (gType.IsEnum)
                        {
                            readCode.AppendFormat("var newData = ({1})reader.ReadByte();\r\n", i, Utils.GetFixFullTypeName(p.PropertyType.GetElementType().FullName));
                        }
                        else if (gType.IsLayoutSequential)
                        {
                            //readCode.AppendFormat("var newData = reader.ReadStruct <{1}>();\r\n", i, Utils.GetFixFullTypeName(p.PropertyType.FullName));

                            AddReadProxyByStruct(gType);
                            readCode.AppendFormat("var newData = {0}ReadProxy.Read(reader);\r\n", gType.Name);
                        }
                        else if (gType.IsClass)
                        {
                            AddReadProxy(gType);
                            readCode.AppendFormat("var newData = {0}ReadProxy.Read(reader);\r\n", gType.Name);
                        }

                        readCode.AppendFormat("ret.{0}.Add(newData);\r\n", p.Name);
                        readCode.AppendLine("}");
                    }
                }
                else if (p.PropertyType.IsClass)
                {
                    AddReadProxy(p.PropertyType);
                    readCode.AppendFormat(" ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.PropertyType.Name);
                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.PropertyType.Name));
                }
            }

            readProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", type.Name)
                .Replace("#TypeFullName#", Utils.GetFixFullTypeName(type.FullName))
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
            int i = 0;
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
                    readCode.AppendFormat("ret.{0} = ({1})reader.ReadByte();\r\n", p.Name, Utils.GetFixFullTypeName(p.FieldType.FullName));
                }
                else if (p.FieldType.IsLayoutSequential)
                {
                    //readCode.AppendFormat("ret.{0} = reader.ReadStruct <{1}>();\r\n", p.Name, Utils.GetFixFullTypeName(p.FieldType.FullName));
                    AddReadProxyByStruct(p.FieldType);
                    readCode.AppendFormat(" ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.FieldType.Name);
                }
                else if (p.FieldType.IsArray)
                {
                    #region 处理数组的读取

                    i++;

                    var arrayType = p.FieldType.GetElementType();

                    readCode.AppendFormat("var len{0} = reader.ReadInt32();\r\n", i);
                    readCode.AppendFormat("var p{0} = new {1}[len{0}];", i, Utils.GetFixFullTypeName(arrayType.FullName));   //  这里只创建值类型

                    readCode.AppendFormat("for(int i =0;i< len{0};i++){{\r\n", i);
                    if (arrayType == typeof(int))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadInt32();\r\n", i);
                    }
                    else if (arrayType == typeof(byte))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadByte();\r\n", i);
                    }
                    else if (arrayType == typeof(long))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadLong64();\r\n", i);
                    }
                    else if (arrayType == typeof(float))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadFloat();\r\n", i);
                    }
                    else if (arrayType == typeof(double))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadFloat();\r\n", i);
                    }
                    else if (arrayType == typeof(bool))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadBoolean();\r\n", i);
                    }
                    else if (arrayType == typeof(string))
                    {
                        readCode.AppendFormat("p{0}[i] = reader.ReadUTF8String();\r\n", i);
                    }
                    else if (arrayType.IsEnum)
                    {
                        readCode.AppendFormat("p{0}[i] = ({1})reader.ReadByte();\r\n", i, Utils.GetFixFullTypeName(p.FieldType.GetElementType().FullName));
                    }
                    else if (arrayType.IsLayoutSequential)
                    {
                        //readCode.AppendFormat("p{0}[i] = reader.ReadStruct <{1}>();\r\n", i, Utils.GetFixFullTypeName(p.FieldType.FullName));
                        AddReadProxyByStruct(arrayType);
                        readCode.AppendFormat("p{1}[i] = {0}ReadProxy.Read(reader);\r\n", arrayType.Name, i);
                    }
                    else if (arrayType.IsClass)
                    {
                        AddReadProxy(arrayType);
                        readCode.AppendFormat("p{1}[i] = {0}ReadProxy.Read(reader);\r\n", arrayType.Name, i);
                    }

                    readCode.AppendLine("}");

                    readCode.AppendFormat("ret.{0} = p{1};\r\n", p.Name, i);

                    #endregion
                }
                else if (p.FieldType.IsGenericType)
                {
                    if (p.FieldType.Name.IndexOf("List") > -1)
                    {
                        //  泛型的列表
                        var gType = p.FieldType.GetGenericArguments()[0];
                        readCode.AppendFormat("ret.{0} = new System.Collections.Generic.List<{1}>();\r\n", p.Name, Utils.GetFixFullTypeName(gType.FullName));
                        readCode.AppendFormat("var len{0} = reader.ReadInt32();;\r\n", i);
                        readCode.AppendFormat("for(int i =0;i< len{0};i++){{\r\n", i);
                        if (gType == typeof(int))
                        {
                            readCode.AppendFormat("var newData = reader.ReadInt32();\r\n", i);
                        }
                        else if (gType == typeof(byte))
                        {
                            readCode.AppendFormat("var newData = reader.ReadByte();\r\n", i);
                        }
                        else if (gType == typeof(long))
                        {
                            readCode.AppendFormat("var newData = reader.ReadLong64();\r\n", i);
                        }
                        else if (gType == typeof(float))
                        {
                            readCode.AppendFormat("var newData = reader.ReadFloat();\r\n", i);
                        }
                        else if (gType == typeof(double))
                        {
                            readCode.AppendFormat("var newData = reader.ReadFloat();\r\n", i);
                        }
                        else if (gType == typeof(bool))
                        {
                            readCode.AppendFormat("var newData = reader.ReadBoolean();\r\n", i);
                        }
                        else if (gType == typeof(string))
                        {
                            readCode.AppendFormat("var newData = reader.ReadUTF8String();\r\n", i);
                        }
                        else if (gType.IsEnum)
                        {
                            readCode.AppendFormat("var newData = ({1})reader.ReadByte();\r\n", i, Utils.GetFixFullTypeName(p.FieldType.GetElementType().FullName));
                        }
                        else if (gType.IsLayoutSequential)
                        {
                            //readCode.AppendFormat("var newData = reader.ReadStruct <{1}>();\r\n", i, Utils.GetFixFullTypeName(p.FieldType.FullName));
                            AddReadProxyByStruct(gType);
                            readCode.AppendFormat("var newData = {0}ReadProxy.Read(reader);\r\n", gType.Name);
                        }
                        else if (gType.IsClass)
                        {
                            AddReadProxy(gType);
                            readCode.AppendFormat("var newData = {0}ReadProxy.Read(reader);\r\n", gType.Name);
                        }

                        readCode.AppendFormat("ret.{0}.Add(newData);\r\n", p.Name);
                        readCode.AppendLine("}");
                    }
                }
                else if (p.FieldType.IsClass)
                {
                    AddReadProxy(p.FieldType);
                    readCode.AppendFormat(" ret.{0} = {1}ReadProxy.Read(reader);\r\n", p.Name, p.FieldType.Name);
                }
                else
                {
                    Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                        classType.Name, type.Name, p.Name, p.FieldType.Name));
                }
            }

            readProxyCode.AppendLine(
                readProxyCodeFormatter.Replace("#TypeName#", type.Name)
                .Replace("#TypeFullName#", Utils.GetFixFullTypeName(type.FullName))
                .Replace("#ReadCode#", readCode.ToString())
                );
        }

        private string readProxyCodeFormatter = @"
    class #TypeName#ReadProxy
    {
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
                #region PacketReader

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

                #endregion
            }

            if (att.MethodType == NetMethodType.ProtocolStruct)
            {
                #region ProtocolStruct

                if (!param[1].ParameterType.IsClass)
                {
                    Logs.Error("{0}.{1} 的第二个参数必须是class类型。", classType.Name, methodinfo.Name);
                    return;
                }

                if (param[1].ParameterType.GetInterface(typeof(IPacketReader).FullName) == null)
                {
                    Logs.Error("{0}.{1} 的第二个参数必须实现 IPacketReader 接口", classType.Name, methodinfo.Name);
                    //  自己实现一个对对象的协议读取类
                    AddReadProxy(param[1].ParameterType);
                    string methodName = Utils.GetFixBeCallProxyName(methodinfo.Name);
                    initCode.AppendFormat("PacketHandlerManager.Register({0}, {1});",
                                          att.OpCode, methodName);
                    initCode.AppendLine();

                    callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", methodName);
                    callCode.AppendLine("{");
                    callCode.AppendFormat(" var obj = {0}ReadProxy.Read(reader);\r\n", param[1].ParameterType.Name);
                    callCode.AppendFormat("module.{0}(obj);", methodName);
                    callCode.AppendLine("}");
                }
                else
                {
                    //  如果对象实现了 IPacketReader 接口，则直接使用，否则则自己生成协议代码
                    string methodName = Utils.GetFixBeCallProxyName(methodinfo.Name);
                    initCode.AppendFormat("PacketHandlerManager.Register({0}, {1});",
                                          att.OpCode, methodName);
                    initCode.AppendLine();

                    callCode.AppendFormat("void {0}(NetState netstate, PacketReader reader)", methodName);
                    callCode.AppendLine("{");
                    callCode.AppendFormat(" var package = DogSE.Library.Common.StaticObjectPool<{0}>.AcquireContent();", param[1].ParameterType.FullName);
                    callCode.AppendLine("package.Read(reader);");
                    callCode.AppendFormat("module.{0}(netstate, package);", methodName);
                    callCode.AppendFormat("DogSE.Library.Common.StaticObjectPool<{0}>.ReleaseContent(package);", param[1].ParameterType.FullName);
                    callCode.AppendLine("}");
                }


                #endregion
            }

            if (att.MethodType == NetMethodType.SimpleMethod)
            {
                #region SimpleMethod
		
                string methodName = Utils.GetFixBeCallProxyName(methodinfo.Name);

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
                    else if (p.ParameterType == typeof(byte))
                    {
                        callCode.AppendFormat("var p{0} = reader.ReadByte();\r\n", i);
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
                    else if (p.ParameterType == typeof(DateTime))
                    {
                        callCode.AppendFormat("var p{0} = new DateTime(reader.ReadLong64());\r\n",i);
                    }
                    else if (p.ParameterType.IsEnum)
                    {
                        callCode.AppendFormat("var p{0} = ({1})reader.ReadByte();\r\n", i, Utils.GetFixFullTypeName(p.ParameterType.FullName));
                    }
                    else if (p.ParameterType.IsLayoutSequential)
                    {
                        //callCode.AppendFormat("var p{0} = reader.ReadStruct <{1}>();\r\n", i, Utils.GetFixFullTypeName(p.ParameterType.FullName));
                        AddReadProxyByStruct(p.ParameterType);
                        callCode.AppendFormat(" var p{1} = {0}ReadProxy.Read(reader);\r\n", p.ParameterType.Name, i);
                    }
                    else if (p.ParameterType.IsArray)
                    {
                        //  数组
                        #region 处理数组的读取
                        var arrayType = p.ParameterType.GetElementType();

                        callCode.AppendFormat("var len{0} = reader.ReadInt32();\r\n", i);
                        callCode.AppendFormat("var p{0} = new {1}[len{0}];", i, Utils.GetFixFullTypeName(arrayType.FullName));   //  这里只创建值类型

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
                        else if (arrayType.IsEnum)
                        {
                            callCode.AppendFormat("p{0}[i] = ({1})reader.ReadByte();\r\n", i, Utils.GetFixFullTypeName(arrayType.FullName));
                        }
                        else if (arrayType.IsLayoutSequential)
                        {
                            //callCode.AppendFormat("p{0}[i] = reader.ReadStruct <{1}>();\r\n", i, Utils.GetFixFullTypeName(arrayType.FullName));
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

                    callCode.AppendFormat("module.{0}(", methodName);
                }
                else
                {
                    //  不需要验证
                    callCode.AppendFormat("module.{0}(", methodName);
                }


                for (int i = 1; i < param.Length; i++)
                    callCode.AppendFormat("p{0},", i);

                if (param.Length > 1)
                    callCode.Remove(callCode.Length - 1, 1);

                callCode.AppendLine(");");
                callCode.AppendLine("}");

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
            ret.Replace("#ClassName#", Utils.GetFixInterfaceName(classType.Name));
            ret.Replace("#FullClassName#", classType.FullName);
            ret.Replace("#InterfaceName#", Utils.GetFixFullTypeName(classType.FullName));
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
                    IProtoclAutoCode pac = new #ClassName#Access();
                    list.Add(pac);

                    pac.SetModule(m as #FullClassName#);
                    pac.PacketHandlerManager = handlers;
                    pac.Init();
                }");

            ret.Replace("#ClassName#", classType.Name);
            ret.Replace("#FullClassName#", classType.FullName);
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

        class ControllerPacketHandler
        {
            public ControllerPacketHandler(NetController net, Base#ClassName#Controller logic)
            {
                PacketHandlerManager = net.PacketHandlers;

                module = logic;
#InitMethod#
            }

        public PacketHandlersBase PacketHandlerManager {get;set;}

        Base#ClassName#Controller module;

#CallMethod#

#ReadProxy#
        }

";
    }

    #region CreateInterfaceCode

    /// <summary>
    /// 创建接口代码
    /// </summary>
    internal class CreateInterfaceCode
    {
        private readonly Type classType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">要创建的方法的类</param>
        public CreateInterfaceCode(Type type)
        {
            classType = type;
        }

        /// <summary>
        /// 添加一个方法
        /// </summary>
        /// <param name="att"></param>
        /// <param name="methodinfo"></param>
        private void AddMethod(NetMethodAttribute att, MethodInfo methodinfo)
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

                string methodName = Utils.GetFixBeCallProxyName(methodinfo.Name);

                StringBuilder methonNameCode = new StringBuilder();
                methonNameCode.AppendFormat("internal abstract  void {0}(", methodName);

                methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(parameterType.FullName), parameterType.Name);


                methonNameCode.Remove(methonNameCode.Length - 1, 1);
                methonNameCode.Append(");");

                callCode.AppendLine(methonNameCode.ToString());

                #endregion
            }

            if (att.MethodType == NetMethodType.SimpleMethod)
            {
                #region SimpleMethod


                string methodName = Utils.GetFixBeCallProxyName(methodinfo.Name);
                StringBuilder methonNameCode = new StringBuilder();
                methonNameCode.AppendFormat("internal abstract void {0}(", methodName);

                for (int i = 1; i < param.Length; i++)
                {
                    var p = param[i];
                    if (p.ParameterType == typeof(int))
                    {
                        methonNameCode.AppendFormat("int {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(byte))
                    {
                        methonNameCode.AppendFormat("byte {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(long))
                    {
                        methonNameCode.AppendFormat("long {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(float))
                    {
                        methonNameCode.AppendFormat("float {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(double))
                    {
                        methonNameCode.AppendFormat("double {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(bool))
                    {
                        methonNameCode.AppendFormat("bool {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(string))
                    {
                        methonNameCode.AppendFormat("string {0},", p.Name);
                    }
                    else if (p.ParameterType == typeof(DateTime))
                    {
                        methonNameCode.AppendFormat("DateTime {0},", p.Name);
                    }
                    else if (p.ParameterType.IsEnum)
                    {
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                    }
                    else if (p.ParameterType.IsLayoutSequential)
                    {
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                    }
                    else if (p.ParameterType.IsArray)
                    {
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                    }
                    else if (p.ParameterType.IsClass)
                    {
                        methonNameCode.AppendFormat("{0} {1},", Utils.GetFixFullTypeName(p.ParameterType.FullName), p.Name);
                    }
                    else
                    {
                        Logs.Error(string.Format("{0}.{1} 存在不支持的参数 {2}，类型未：{3}",
                            classType.Name, methodinfo.Name, p.Name, p.ParameterType.Name));
                    }

                }
                if (param.Length > 1)
                    methonNameCode.Remove(methonNameCode.Length - 1, 1);

                methonNameCode.Append(");");

                callCode.AppendLine(methonNameCode.ToString());

                #endregion
            }
        }

        private StringBuilder callCode = new StringBuilder();


        private string GetCode()
        {
            var ret = new StringBuilder();

            ret.Append(CodeBase);
            ret.Replace("#ClassName#", Utils.GetFixInterfaceName(classType.Name));
            ret.Replace("#FullClassName#", classType.FullName);
            //ret.Replace("", s_version.ToString());
            //ret.Replace("#InitMethod#", initCode.ToString());
            ret.Replace("#CallMethod#", callCode.ToString());
            //ret.Replace("#ProxyCode#", writerProxyCode.ToString());

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

        private string CodeBase = @"

    /// <summary>
    /// #ClassName#
    /// </summary>
    
    public abstract class Base#ClassName#Controller
    {
#CallMethod#        
    }
";
    }

    #endregion


    /// <summary>
    /// 客户端逻辑协议代码生成器
    /// 这里主要是生成一个接口对象和一个对接口操作的代理类
    /// </summary>
    public class ClientLogicProtocolGeneration
    {
        /// <summary>
        /// 创建代码
        /// </summary>
        /// <remarks>
        /// 这里需要输出3组文件
        /// 1.客户端接口
        /// 2.代理类
        /// 3.代理类和接口的绑定
        /// </remarks>
        /// <param name="dllFile"></param>
        /// <param name="outFolder">输出目录</param>
        /// <param name="nameSpace">基础的命名空间</param>
        public static void CreateCode(string dllFile, string outFolder, string nameSpace)
        {
            StringBuilder interfaceCode = new StringBuilder();

            var dll = Assembly.LoadFrom(dllFile);
            StringBuilder proxyregBuilder = new StringBuilder();

            foreach (var type in dll.GetTypes())
            {
                if (type.IsInterface)
                {
                    var i = type.GetCustomAttributes(typeof(ClientInterfaceAttribute), true);
                    if (i.Length > 0)
                    {
                        //  将接口代码的代码重新复制过去
                        var code = new CreateInterfaceCode(type).CreateCode();
                        interfaceCode.Append(code);

                        //  生成客户端解包代码并写入对应文件
                        code = new CreateReadCode(type).CreateCode();

                        string typeName = Utils.GetFixInterfaceName(type.Name);

                        var context = FileCodeBase
                            .Replace("#code#", code)
                            .Replace("#namespace#", nameSpace)
                            .Replace("#ClassName#", typeName)
                            .Replace("`", "\"");

                        string fileName = Path.Combine(outFolder, string.Format(@"Controller\{0}\{0}Controller.Net.cs", typeName));
                        var fi = new FileInfo(fileName);
                        if (!fi.Directory.Exists)
                            fi.Directory.Create();

                        File.WriteAllText(fileName, context, Encoding.UTF8);
                    }
                }
            }
            var fileContext = InterfaceCodeBase
    .Replace("#code#", interfaceCode.ToString())
    .Replace("#namespace#", nameSpace)
    .Replace("`", "\"");

            var file = Path.Combine(outFolder, @"Controller\Auto\LogicInterface.cs");
            new FileInfo(file).Directory.Create();

            File.WriteAllText(file, fileContext, Encoding.UTF8);
        }

        private const string InterfaceCodeBase = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace #namespace#.Controller
{
#code#
}

";


        const string FileCodeBase = @"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Client.Core.Net;
using DogSE.Client.Core.Task;

namespace #namespace#.Controller.#ClassName#
{


    partial class #ClassName#Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=`net`></param>
        public #ClassName#Controller(NetController net)
        {
            nc = net;
            new ControllerPacketHandler(net, this);
        }

        private NetController nc;


        private NetState NetState
        {
            get
            {
                return nc.NetState;
            }
        }

#code#

    }


}


";
    }
}
