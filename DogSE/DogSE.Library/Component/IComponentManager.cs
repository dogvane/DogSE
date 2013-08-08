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
        /// <param name="componentId"></param>
        /// <param name="component"></param>
        void RegisterComponent<T>(string componentId, T component) where T : class;

        /// <summary>
        /// 获得一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId"></param>
        /// <returns></returns>
        T GetComponent<T>(string componentId) where T : class;

        /// <summary>
        /// 释放组件的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ReleaseComponent<T>();
    }
}
