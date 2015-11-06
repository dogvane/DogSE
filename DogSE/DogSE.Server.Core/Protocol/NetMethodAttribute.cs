using System;
using System.ComponentModel;

namespace DogSE.Server.Core.Protocol
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
    /// 标示忽略不进行序列化的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
        
    }

    /// <summary>
    /// 网络回调方法的参数
    /// </summary>
    public class NetMethodAttribute:Attribute
    {
        /// <summary>
        /// 网络回调方法
        /// </summary>
        /// <param name="opcode">消息码</param>
        /// <param name="type">消息的处理方法</param>
        /// <param name="isVerifyLogin">是否进行登录验证，默认是进行的，只有登录等极少数的消息是不需要验证的</param>
        public NetMethodAttribute(ushort opcode, NetMethodType type, bool isVerifyLogin = true)
        {
            OpCode = opcode;
            MethodType = type;
            IsVerifyLogin = isVerifyLogin;
        }

        /// <summary>
        /// 网络回调方法
        /// </summary>
        /// <param name="opcode">消息码</param>
        /// <param name="type">消息的处理方法</param>
        /// <param name="taskType">任务分类</param>
        /// <param name="isVerifyLogin">是否进行登录验证，默认是进行的，只有登录等极少数的消息是不需要验证的</param>
        public NetMethodAttribute(ushort opcode, NetMethodType type, TaskType taskType, bool isVerifyLogin = true)
        {
            OpCode = opcode;
            MethodType = type;
            IsVerifyLogin = isVerifyLogin;
            TaskType = taskType;
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

        /// <summary>
        /// 任务类型
        /// </summary>
        /// <remarks>
        /// 目前任务分类是一个枚举值，有Main,Low,Assist
        /// 决定则这个消息对应的业务逻辑代码在那个线程里执行
        /// </remarks>
        public TaskType TaskType { get; private set; }
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

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// 主线程任务
        /// </summary>
        Main = 0,

        /// <summary>
        /// 优先级低的线程
        /// 会在某些极端或者特殊情况下数据包会抛弃
        /// 例如：聊天
        /// </summary>
        Low = 1,

        /// <summary>
        /// 辅助线程
        /// 通常和主线程无关的操作，但是又会对主线程产生影响的操作可以放这里
        /// 但是， 涉及到玩家数据变更的任务，还是需要在主线程里处理
        /// 或者可以先在这里进行初步处理，然后再把后面的数据修改的操作抛到主线程任务执行
        /// </summary>
        Assist = 2,
    }
}
