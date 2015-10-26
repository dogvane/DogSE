using DogSE.Library.Component;

#region zh-CHS 2006 - 2010 DemoSoft 团队 | en 2006-2010 DemoSoft Team

//     NOTES
// ---------------
//
// This file is a part of the MMOSE(Massively Multiplayer Online Server Engine) for .NET.
//
//                              2006-2010 DemoSoft Team
//
//
// First Version : by H.Q.Cai - mailto:caihuanqing@hotmail.com

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published
 *   by the Free Software Foundation; either version 2.1 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region zh-CHS 包含名字空间 | en Include namespace

#endregion

namespace DogSE.Client.Core.Net
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NetState : IComponentManager
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
            m_componentManager.RegisterComponent(componentId, component);
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
        /// 释放组件的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ReleaseComponent<T>()
        {
            m_componentManager.ReleaseComponent<T>();
        }

        #endregion

        /// <summary>
        /// 和网络连接关联的对象
        /// </summary>
        public object Player { get; set; }

        /// <summary>
        /// 获得网络连接的ip地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            return m_Socket.RemoteOnlyIP;
        }
    }
}
#endregion