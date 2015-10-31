namespace DogSE.Client.Core.Task
{
    /// <summary>
    /// 一个用于执行的任务接口
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 执行一个任务
        /// </summary>
        void Execute();

        /// <summary>
        /// 和任务相关的线程性能对象
        /// </summary>
        ITaskProfile TaskProfile { get; }

        /// <summary>
        /// 释放和任务相关的资源
        /// </summary>
        void Release();

        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="runTick"></param>
        /// <param name="isError"></param>
        void WriteLog(long runTick, bool isError);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ITaskProfile
    {
        /// <summary>
        /// 调用次数
        /// </summary>
        long Count { get; }

        /// <summary>
        /// 执行时间的ticks，注意，不是ms，转换为ms需要除1000
        /// </summary>
        long ElapsedTicks { get; }

        /// <summary>
        /// 执行的错误次数
        /// </summary>
        long Error { get; }

        /// <summary>
        /// 追加一次记录
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="isError"></param>
        void Append(long ticks, bool isError = false);
    }
}
