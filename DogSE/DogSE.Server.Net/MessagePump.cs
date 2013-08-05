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
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Common.Log;
using Demo.Mmose.Core.Timer;
using Demo.Mmose.Core.Util;
using Demo.Mmose.Core.World;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 数据信息包抽取处理
    /// </summary>
    [MultiThreadedSupport( "zh-CHS", "当前的类所有成员都可锁定,支持多线程操作" )]
    public class MessagePump
    {
        #region zh-CHS 共有属性 | en Public Properties

        #region zh-CHS Listeners属性 | en Listeners Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 多个监听的端口
        /// </summary>
        private Listener[] m_Listeners = new Listener[0];
        #endregion
        /// <summary>
        /// 监听的端口
        /// </summary>
        public Listener[] Listeners
        {
            get { return m_Listeners; }
        }

        #region zh-CHS 共有方法 | en Public Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 只锁定添加操作(因为是数组，其它的地方就可以不用锁定的)
        /// </summary>
        private SpinLock m_OnlyLockAddListener = new SpinLock();
        #endregion
        /// <summary>
        /// 添加多个监听端口的数据传过来
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener( Listener listener )
        {
            if ( listener == null )
                throw new ArgumentNullException( "listener", "MessagePump.AddListener(...) - listener == null error!" );

            if ( listener.World != null && listener.World != World )
                throw new ArgumentException( "MessagePump.AddListener(...) - listener.World != null && listener.World != this.World error!", "listener.World" );

            // 检查是否有相同的监听器
            Listener[] listenerArray = m_Listeners;
            for ( int iIndex = 0; iIndex < listenerArray.Length; iIndex++ )
            {
                Listener itemListener = listenerArray[iIndex];
                if ( itemListener == listener )
                    return;
            }

            SpinLockEx.ReliableEnter( ref m_OnlyLockAddListener );
            try
            {
                // 创建新的Listener数组,添加数据,交换数组数据,不需要锁定,没有引用时自动会回收数据
                Listener[] tempListener = new Listener[m_Listeners.Length + 1];

                for ( int iIndex = 0; iIndex < m_Listeners.Length; ++iIndex )
                    tempListener[iIndex] = m_Listeners[iIndex];

                tempListener[m_Listeners.Length] = listener;
                listener.World = m_World;

                m_Listeners = tempListener;
            }
            finally
            {
                m_OnlyLockAddListener.Exit();
            }
        }

        #endregion

        #endregion

        #region zh-CHS Connecters属性 | en Connecters Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 多个监听的端口
        /// </summary>
        private Connecter[] m_Connecters = new Connecter[0];
        #endregion
        /// <summary>
        /// 连接的端口
        /// </summary>
        public Connecter[] Connecters
        {
            get { return m_Connecters; }
        }

        #region zh-CHS 共有方法 | en Public Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 只锁定添加操作(因为是数组，其它的地方就可以不用锁定的)
        /// </summary>
        private SpinLock m_OnlyLockAddConnecter = new SpinLock();
        #endregion
        /// <summary>
        /// 添加多个连接端口的数据传过来
        /// </summary>
        public void AddConnecter( Connecter connecter )
        {
            if ( connecter == null )
                throw new ArgumentNullException( "connecter", "MessagePump.AddConnecter(...) - connecter == null error!" );

            if ( connecter.World != null && connecter.World != World )
                throw new ArgumentException( "connecter.World", "MessagePump.AddConnecter(...) - connecter.World != null && connecter.World != this.World error!" );

            // 检查是否有相同的连接器
            Connecter[] connecterArray = m_Connecters;
            for ( int iIndex = 0; iIndex < connecterArray.Length; iIndex++ )
            {
                Connecter itemConnecter = connecterArray[iIndex];
                if ( itemConnecter == connecter )
                    return;
            }

            SpinLockEx.ReliableEnter( ref m_OnlyLockAddConnecter );
            {
                // 创建新的Listener数组,添加数据,交换数组数据,不需要锁定,没有引用时自动会回收数据
                Connecter[] tempConnecter = new Connecter[m_Connecters.Length + 1];

                for ( int iIndex = 0; iIndex < m_Connecters.Length; ++iIndex )
                    tempConnecter[iIndex] = m_Connecters[iIndex];

                tempConnecter[m_Connecters.Length] = connecter;
                connecter.World = m_World;

                m_Connecters = tempConnecter;
            }
            m_OnlyLockAddConnecter.Exit();
        }
        #endregion

        #endregion

        #region zh-CHS World属性 | en World Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 管理当前的世界服务
        /// </summary>
        private WorldBase m_World;
        #endregion
        /// <summary>
        /// 管理当前的世界服务
        /// </summary>
        public WorldBase World
        {
            get { return m_World; }
            internal set
            {
                m_World = value;

                // 重新设置Listener数据的BaseWorld
                Listener[] listeners = m_Listeners;
                for ( int iIndex = 0; iIndex < listeners.Length; iIndex++ )
                    listeners[iIndex].World = m_World;                    

                // 重新设置Connecter数据的BaseWorld
                Connecter[] connecters = m_Connecters;
                for ( int iIndex = 0; iIndex < connecters.Length; iIndex++ )
                    connecters[iIndex].World = m_World;
            }
        }

        #endregion

        #endregion

        #region zh-CHS 内部方法 | en Internal Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 当前需要处理的NetState集合
        /// </summary>
        private ConcurrentQueue<NetState> m_NetStateQueue = new ConcurrentQueue<NetState>();
        #endregion
        /// <summary>
        /// 时间片的处理
        /// </summary>
        internal void Slice()
        {
            // 检查有多少新的连接用户
            CheckListener();

            // 检查有多少新的连出用户
            CheckConnecter();

            NetState netState = null;
            if ( m_NetStateQueue.TryDequeue( out netState ) == false )
                return;

            // 如果没有需要处理的数据则返回
            if ( netState == null )
                return;

            // 处理接收到的数据
            if ( netState.Running == true )
            {
                ReceiveQueue receiveQueueBuffer = netState.ReceiveBuffer;
                if ( receiveQueueBuffer == null )
                    throw new ArgumentNullException( "receiveQueueBuffer", "MessagePump.Slice(...) - receiveQueueBuffer == null error!" );

                // 检测是否已经在处理数据中,防止多线程中多次处理里面的数据
                if ( receiveQueueBuffer.InProcessReceive() == false )
                    return;

                // 累积过多，会导致线程堵塞，全都在处理自我的数据包逻辑
                if ( netState.ReceiveBuffer.Length >= netState.ReceiveCachedMaxSize )
                {
                    netState.Dispose();

                    // 需要注释
                    Logs.WriteLine( true, LogMessageType.MSG_WARNING, "NetState[{0}] Receive(...) - ReceiveBufferLength[{1}] >= ReceiveCachedMaxSize[{2}] warning (接收缓存的数据包过大)!", ToString(), netState.SendQueue.WaitSendSize, netState.ReceiveCachedMaxSize );
                }
                else
                    OnProcessReceive( netState );

                // 已经结束处理
                receiveQueueBuffer.OutProcessReceive();
            }

            // 表示当前已不再处理列表中(减少处理列表的长度)
            netState.OutProcessQueue();

            // 再次检测是否还有没处理完接受到的数据,如果有,再次进入处理列表(可能是半包或还有没来得急处理的数据,等待数据的完整,1秒以后再次处理,仅调用一次)
            if ( netState.Running == true )
            {
                if ( netState.ReceiveBuffer.Length > 0 )
                    TimeSlice.StartTimeSlice( TimeSpan.FromSeconds( 1.0 ), OnceAgainReceive, netState );
            }

            // 有数据过来需要发送全局信号处理数据包
            if ( m_NetStateQueue.IsEmpty == false )
                m_World.SendWorldSignal();
        }

        #region zh-CHS 再次进入处理数据的时间片回调函数 | en TimeSlice
        /// <summary>
        /// 再次处理没处理完全的数据(仅调用一次)
        /// </summary>
        /// <param name="netState"></param>
        private void OnceAgainReceive( NetState netState )
        {
            if ( netState.Running )
            {
                if ( netState.ReceiveBuffer.Length > 0 )
                    OnReceive( netState );
            }
        }
        #endregion

        /// <summary>
        /// 网络上面有数据过来
        /// </summary>
        /// <param name="netState"></param>
        internal void OnReceive( NetState netState )
        {
            // 表示当前已加入在处理列表中(减少处理列表的长度)
            if ( netState.InProcessQueue() == false )
                return;
           
            m_NetStateQueue.Enqueue( netState );

            Debug.Assert( m_World != null, "MessagePump.OnReceive(...) - m_World == null error!" );

            // 有数据过来需要发送全局信号处理数据包
            m_World.SendWorldSignal();
        }
        #endregion

        #region zh-CHS 私有方法 | en Private Methods
        /// <summary>
        /// 检查是否有新的客户端接过来
        /// </summary>
        private void CheckListener()
        {
            for ( int iIndex = 0; iIndex < m_Listeners.Length; iIndex++ )
            {
                Listener listener = m_Listeners[iIndex];

                // 获取连接的客户端
                ClientSocketManager[] acceptedManager = listener.Slice();

                for ( int iIndex2 = 0; iIndex2 < acceptedManager.Length; iIndex2++ )
                {
                    ClientSocketManager clientSocketManager = acceptedManager[iIndex2];
                    if ( clientSocketManager == null )
                        continue;

                    // 把连接过来的客户端放置入NetState中,当前的实例会保存在NetState.Instances中
                    NetState netState = new NetState( clientSocketManager, this );

                    OnNetStateInit( netState );

                    netState.Start();
                }
            }
        }

        /// <summary>
        /// 检查是否有新的客户端接出来
        /// </summary>
        private void CheckConnecter()
        {
            for ( int iIndex = 0; iIndex < m_Connecters.Length; iIndex++ )
            {
                Connecter connecter = m_Connecters[iIndex];

                // 检查是否已经处理过
                if ( connecter.IsNeedSlice == false )
                    continue;

                // 获取连接出去的客户端
                ClientSocketManager clientSocketManager = connecter.Slice();
                if ( clientSocketManager != null )
                {
                    // 把连接出来的客户端放置入NetState中,当前的实例会保存在NetState.Instances中
                    NetState netState = new NetState( clientSocketManager, this );

                    OnNetStateInit( netState );

                    netState.Start();
                }
            }
        }
        #endregion

        #region zh-CHS 内部的事件处理函数 | en Internal Event Handlers
        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        /// <param name="netState"></param>
        internal void OnProcessReceive( NetState netState )
        {
            EventHandler<NetStateEventArgs> tempEventArgs = m_EventProcessReceive;
            if ( tempEventArgs != null )
            {
                NetStateEventArgs netStateEventArgs = new NetStateEventArgs( netState );
                tempEventArgs( this, netStateEventArgs );
            }
        }

        /// <summary>
        /// 创建了新的NetState需要的初始化
        /// </summary>
        internal void OnNetStateInit( NetState newNetState )
        {
            EventHandler<NetStateInitEventArgs> tempEventArgs = m_EventNetStateCreate;
            if ( tempEventArgs != null )
            {
                NetStateInitEventArgs netStateInitEventArgs = new NetStateInitEventArgs( newNetState );
                tempEventArgs( this, netStateInitEventArgs );
            }
        }
        #endregion

        #region zh-CHS 共有事件 | en Public Event
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private EventHandler<NetStateEventArgs> m_EventProcessReceive;
        /// <summary>
        /// 
        /// </summary>
        private SpinLock m_LockEventProcessReceive = new SpinLock();
        #endregion
        /// <summary>
        /// 数据处理事件
        /// </summary>
        [MultiThreadedWarning( "zh-CHS", "单一用户只在单一的线程内回调:(防止数据包顺序错误,单一用户不存在多线程问题)警告!" )]
        public event EventHandler<NetStateEventArgs> EventProcessReceive
        {
            add
            {
                SpinLockEx.ReliableEnter( ref m_LockEventProcessReceive );
                {
                    m_EventProcessReceive += value;
                }
                m_LockEventProcessReceive.Exit();
            }
            remove
            {
                SpinLockEx.ReliableEnter( ref m_LockEventProcessReceive );
                {
                    m_EventProcessReceive -= value;
                }
                m_LockEventProcessReceive.Exit();
            }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private EventHandler<NetStateInitEventArgs> m_EventNetStateCreate;
        /// <summary>
        /// 
        /// </summary>
        private SpinLock m_LockEventNetStateCreate = new SpinLock();
        #endregion
        /// <summary>
        /// 当创建一个新的NetState时的回调
        /// </summary>
        [MultiThreadedWarning( "zh-CHS", "单一用户只在单一的线程内回调:(单一用户不存在多线程问题)警告!" )]
        public event EventHandler<NetStateInitEventArgs> EventNetStateCreate
        {
            add
            {
                SpinLockEx.ReliableEnter( ref m_LockEventNetStateCreate );
                {
                    m_EventNetStateCreate += value;
                }
                m_LockEventNetStateCreate.Exit();
            }
            remove
            {
                SpinLockEx.ReliableEnter( ref m_LockEventNetStateCreate );
                {
                    m_EventNetStateCreate -= value;
                }
                m_LockEventNetStateCreate.Exit();
            }
        }
        #endregion
    }
}
#endregion
