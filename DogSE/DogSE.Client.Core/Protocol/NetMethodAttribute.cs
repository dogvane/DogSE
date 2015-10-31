using System;
using System.ComponentModel;

namespace DogSE.Client.Core.Protocol
{

    /// <summary>
    /// 网络返回的描叙信息
    /// </summary>
    public class NetReturnDescription : DescriptionAttribute
    {
        /// <summary>
        /// 网络返回的描叙信息
        /// </summary>
        /// <param name="description"></param>
        public NetReturnDescription(string description)
            : base(description)
        {

        }
    }

    /// <summary>
    /// 客户端的接口定义标签
    /// 只用来标记，给协议生成工具找到对应的接口
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ClientInterfaceAttribute : Attribute
    {
        
    }

    /// <summary>
    /// 网络回调方法的参数
    /// </summary>
    public class NetMethodAttribute:Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="type">消息的处理方法</param>
        /// <param name="isVerifyLogin">是否进行登录验证，默认是进行的，只有登录等极少数的消息是不需要验证的</param>
        public NetMethodAttribute(ushort opcode, NetMethodType type, bool isVerifyLogin = true)
        {
            OpCode = opcode;
            MethodType = type;
            IsVerifyLogin = isVerifyLogin;
        }

        /// <summary>
        /// 消息码
        /// </summary>
        public ushort OpCode { get; private set; }

        /// <summary>
        /// 方法类型
        /// </summary>
        public NetMethodType MethodType { get; private set; }

        /// <summary>
        /// 是否进行登录验证
        /// </summary>
        public bool IsVerifyLogin { get; private set; }
    }

    /// <summary>
    /// 网络方法的生成类型
    /// </summary>
    public enum NetMethodType
    {
        /// <summary>
        /// 方法里一共2个参数
        /// 第二个参数为使用的是数据流，需要自己来解析数据流内容
        /// </summary>
        PacketReader  = 0,

        /// <summary>
        /// 方法里一共2个参数
        /// 第二个参数为已经定义过解析协议的数据包流对象
        /// </summary>
        ProtocolStruct = 1,

        /// <summary>
        /// 简单方法，由系统自动帮忙负责解析协议
        /// </summary>
        SimpleMethod = 2,
    }
}
