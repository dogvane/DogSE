using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Library.Component;

namespace TradeAge.Server.Entity.Character
{
    public partial class Player : IComponentManager
    {
        #region IComponentManager 成员

        private readonly ComponentManager m_componentManager = new ComponentManager();

        /// <summary>
        /// 注册一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId">组件ID</param>
        /// <param name="component">组件实例（非空）</param>
        public void RegisterComponent<T>(string componentId, T component) where T : class
        {
            m_componentManager.RegisterComponent<T>(componentId, component);
        }

        /// <summary>
        /// 注册一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">组件实例（非空）</param>
        public void RegisterComponent<T>(T component) where T : class
        {
            m_componentManager.RegisterComponent<T>(typeof(T).Name, component);
        }

        /// <summary>
        /// 获得一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId">组件ID</param>
        /// <returns></returns>
        public T GetComponent<T>(string componentId) where T : class
        {
            return m_componentManager.GetComponent<T>(componentId);
        }


        /// <summary>
        /// 获得一个组件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : class
        {
            return m_componentManager.GetComponent<T>(typeof(T).Name);
        }

        /// <summary>
        /// 释放组件的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ReleaseComponent<T>()
        {
            m_componentManager.ReleaseComponent<T>();
        }

        #endregion


		/// <summary>
		/// 玩家自身的组件Id
		/// </summary>
        public const string ComponentId = "Player";
    }
}
