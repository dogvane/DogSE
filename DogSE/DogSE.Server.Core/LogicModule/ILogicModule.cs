
namespace DogSE.Server.Core.LogicModule
{
    /// <summary>
    /// 逻辑模块公开接口
    /// </summary>
    public interface ILogicModule
    {
        /// <summary>
        /// 模块的ID（名字）
        /// </summary>
        string ModuleId { get; }

        /// <summary>
        /// 模块初始化中
        /// 和模块相关的控制器 Controller 在这里初始化
        /// </summary>
        void Initializationing();

        /// <summary>
        /// 模块初始化结束
        /// 和模块相关的事件在这里初始化
        /// </summary>
        void Initializationed();

        /// <summary>
        /// 重新加载模板（内部重新初始化）
        /// </summary>
        void ReLoadTemplate();

        /// <summary>
        /// 服务器停止时释放资源
        /// </summary>
        void Release();
    }
}
