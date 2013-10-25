namespace DogSE.Library.Component
{
    /// <summary>
    /// 组件模式模块
    /// </summary>
    public interface IComponentManager
    {
        /// <summary>
        /// 注册一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId">组件ID</param>
        /// <param name="component">组件实例（非空）</param>
        void RegisterComponent<T>(string componentId, T component) where T : class;

        /// <summary>
        /// 获得一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId">组件ID</param>
        /// <returns></returns>
        T GetComponent<T>(string componentId) where T : class;

        /// <summary>
        /// 释放组件的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ReleaseComponent<T>();
    }
}
