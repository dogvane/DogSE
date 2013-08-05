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
using Demo.Mmose.Core.Common.Component;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NetState : IComponentManager
    {
        #region zh-CHS IComponent接口实现 | en IComponent Interface Implementation

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private ComponentManager m_ComponentManager = new ComponentManager();
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentMessage"></param>
        public void OnHandleComponentMessage( ComponentMessage componentMessage )
        {
            m_ComponentManager.OnHandleComponentMessage( componentMessage );
        }

        #endregion

        #region zh-CHS IComponentHandler接口实现 | en IComponentHandler Interface Implementation
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId"></param>
        /// <param name="component"></param>
        public void RegisterComponent<T>( ComponentId componentId, T component ) where T : class, IComponent
        {
            m_ComponentManager.RegisterComponent<T>( componentId, component );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public T GetComponent<T>( ComponentId componentId ) where T : class, IComponent
        {
            return m_ComponentManager.GetComponent<T>( componentId );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="componentMessage"></param>
        public void SubScribeComponentMessage( ComponentMessage componentMessage, ComponentId componentId )
        {
            m_ComponentManager.SubScribeComponentMessage( componentMessage, componentId );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentMessage"></param>
        public void PostComponentMessage( ComponentMessage componentMessage )
        {
            m_ComponentManager.PostComponentMessage( componentMessage );
        }
        #endregion
    }
}
#endregion