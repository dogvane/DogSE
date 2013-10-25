using System;
using System.Collections.Concurrent;

namespace DogSE.Library.Component
{
    /// <summary>
    /// 组件管理器的实现模式
    /// </summary>
    public class ComponentManager:IComponentManager
    {
        private readonly ConcurrentDictionary<string, object> m_ComponentDictionary =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 注册一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId"></param>
        /// <param name="component"></param>
        public void RegisterComponent<T>(string componentId, T component) where T : class
        {
            if (string.IsNullOrEmpty(componentId))
                throw new ArgumentNullException("componentId");

            if (component == null)
                throw new ArgumentNullException("component");

            m_ComponentDictionary[componentId] = component;
        }

        /// <summary>
        /// 获得一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public T GetComponent<T>(string componentId) where T : class
        {
            if (string.IsNullOrEmpty(componentId))
                throw new ArgumentNullException("componentId");

            object value;
            if (m_ComponentDictionary.TryGetValue(componentId, out value))
                return value as T;

            return null;
        }

        /// <summary>
        /// 释放组件资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ReleaseComponent<T>()
        {
            m_ComponentDictionary.Clear();
        }

        /// <summary>
        /// 清理所有数据（为GC用，注意使用场所）
        /// </summary>
        public void Clear()
        {
            m_ComponentDictionary.Clear();
        }
    }
}
