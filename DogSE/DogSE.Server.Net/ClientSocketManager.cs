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
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Common.Atom;
using Demo.Mmose.Net;
#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 连接过来的客户端
    /// </summary>
    [MultiThreadedSupport( "zh-CHS", "当前的类所有成员都可锁定,支持多线程操作" )]
    public sealed class ClientSocketManager
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 
        /// </summary>
        internal ClientSocketManager( Listener listener, ServiceHandler serviceHandler, ReceiveQueue receiveBuffer )
        {
            if ( listener == null )
                throw new ArgumentNullException( "listener", "ClientSocketHandler.ClientSocketManager(...) - listener == null error!" );

            if ( serviceHandler == null )
                throw new ArgumentNullException( "serviceHandler", "ClientSocketHandler.ClientSocketManager(...) - serviceHandle == null error!" );

            if ( receiveBuffer == null )
                throw new ArgumentNullException( "receiveBuffer", "ClientSocketHandler.ClientSocketManager(...) - receiveBuffer == null error!" );

            m_Listener = listener;
            m_ServiceHandle = serviceHandler;
            m_ReceiveBuffer = receiveBuffer;
            {
                // 清空数据
                m_ReceiveBuffer.Clear();
            }

            m_ServiceHandle.EventProcessData += OnListenerProcessMessageBlock;
            m_ServiceHandle.EventDisconnect += OnListenerDisconnect;

            // 初始化数据 表示还没调用过Free(...)函数
            m_LockFree.SetValid();
        }

        /// <summary>
        /// 
        /// </summary>
        internal ClientSocketManager( Connecter connecter, ConnectHandler connectHandler, ReceiveQueue receiveBuffer  )
        {
            if ( connecter == null )
                throw new ArgumentNullException( "ClientSocketHandler.ClientSocketManager(...) - listener == null error!" );

            if ( connectHandler == null )
                throw new ArgumentNullException( "ClientSocketHandler.ClientSocketManager(...) - serviceHandle == null error!" );

            if ( receiveBuffer == null )
                throw new ArgumentNullException( "receiveBuffer", "ClientSocketHandler.ClientSocketManager(...) - receiveBuffer == null error!" );

            m_Connecter = connecter;
            m_ConnectHandle = connectHandler;
            m_ReceiveBuffer = receiveBuffer;
            {
                // 清空数据
                m_ReceiveBuffer.Clear();
            }

            // 初始化数据 表示还没调用过Free(...)函数
            m_LockFree.SetValid();
        }
        #endregion

        #region zh-CHS 内部属性 | en Internal Properties

        #region zh-CHS 内部 Listener 属性 | en Internal  Listener Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 主监听处理
        /// </summary>
        private ServiceHandler m_ServiceHandle;
        #endregion
        /// <summary>
        /// 监听的主处理
        /// </summary>
        internal ServiceHandler ListenerSocket
        {
            get { return m_ServiceHandle; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private volatile Listener m_Listener;
        #endregion
        /// <summary>
        /// 监听的主操作
        /// </summary>
        internal Listener Listener
        {
            get { return m_Listener; }
        }
        #endregion

        #region zh-CHS 内部 Connecter 属性 | en Internal Connecter Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 主连接处理
        /// </summary>
        private ConnectHandler m_ConnectHandle;
        #endregion
        /// <summary>
        /// 连接的主处理
        /// </summary>
        internal ConnectHandler ConnecterSocket
        {
            get { return m_ConnectHandle; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private volatile Connecter m_Connecter;
        #endregion
        /// <summary>
        /// 连接的主操作
        /// </summary>
        internal Connecter Connecter
        {
            get { return m_Connecter; }
        }
        #endregion

        #endregion

        #region zh-CHS 共有属性 | en Public Properties
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.IsOnline;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.IsOnline;
                else
                    throw new Exception( "ClientSocketHandler.IsOnline(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.RemotePort;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.RemotePort;
                else
                    throw new Exception( "ClientSocketHandler.RemotePort(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 第一次数据包的时间
        /// </summary>
        public DateTime FirstTime
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.FirstTime;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.FirstTime;
                else
                    throw new Exception( "ClientSocketHandler.FirstTime(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 远程的地址
        /// </summary>
        public string RemoteOnlyIP
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.RemoteOnlyIP;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.RemoteOnlyIP;
                else
                    throw new Exception( "ClientSocketHandler.RemoteOnlyIP(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 在线时间
        /// </summary>
        public TimeSpan OnlineTime
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.OnlineTime;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.OnlineTime;
                else
                    throw new Exception( "ClientSocketHandler.OnlineTime(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 客户段的地址与端口
        /// </summary>
        public string Address
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.ClientAddress;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.ServerAddress;
                else
                    throw new Exception( "ClientSocketHandler.Address(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 消息包的总数量
        /// </summary>
        public long MessageBlockNumbers
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.MessageBlockNumbers;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.MessageBlockNumbers;
                else
                    throw new Exception( "ClientSocketHandler.MessageBlockNumbers(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 60秒内的数据包的数量
        /// </summary>
        public long MessageBlockNumbers60sec
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.MessageBlockNumbers60sec;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.MessageBlockNumbers60sec;
                else
                    throw new Exception( "ClientSocketHandler.MessageBlockNumbers60sec(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 最后数据包的时间
        /// </summary>
        public DateTime LastMessageBlockTime
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.LastMessageBlockTime;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.LastMessageBlockTime;
                else
                    throw new Exception( "ClientSocketHandler.LastMessageBlockTime(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 数据包间隔的时间
        /// </summary>
        public TimeSpan MessageBlockSpacingTime
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.MessageBlockSpacingTime;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.MessageBlockSpacingTime;
                else
                    throw new Exception( "ClientSocketHandler.MessageBlockSpacingTime(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        /// <summary>
        /// 最大可等待的发送包但没发送完的数据包的数量
        /// </summary>
        public int PendingMaxSendCount
        {
            get
            {
                if ( m_ServiceHandle != null )
                    return m_ServiceHandle.PendingMaxSendCount;
                else if ( m_ConnectHandle != null )
                    return m_ConnectHandle.PendingMaxSendCount;
                else
                    throw new Exception( "ClientSocketHandler.PendingMaxSendCount(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
            set
            {
                if ( m_ServiceHandle != null )
                    m_ServiceHandle.PendingMaxSendCount = value;
                else if ( m_ConnectHandle != null )
                    m_ConnectHandle.PendingMaxSendCount = value;
                else
                    throw new Exception( "ClientSocketHandler.PendingMaxSendCount(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
            }
        }

        #region zh-CHS NetState属性 | en NetState Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private NetState m_NetState;
        #endregion
        /// <summary>
        /// 设置管理当前的世界服务
        /// </summary>
        public NetState NetState
        {
            get { return m_NetState; }
            set
            {
                if ( m_NetState == value )
                    return;
                else
                    m_NetState = value;

                // 检查是否在线状态
                if ( IsOnline == false )
                {
                    DisconnectSignal();
                    return;
                }

                // 检查是否已经有数据过来
                if ( m_ReceiveBuffer.Length > 0 )
                    ReceiveSignal();
            }
        }

        #endregion

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 接受到的数据包
        /// </summary>
        private ReceiveQueue m_ReceiveBuffer;
        #endregion
        /// <summary>
        /// 接受到的数据存放处
        /// </summary>
        public ReceiveQueue ReceiveBuffer
        {
            get { return m_ReceiveBuffer; }
        }
        #endregion

        #region zh-CHS 内部方法 | en Internal Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private LockCheck m_LockFree = new LockCheck( false );
        #endregion
        /// <summary>
        /// 内部断开(调用后此类不得再进行任何处理,因为已返回进内存池)
        /// </summary>
        internal void Free()
        {
            // 检测有没有调用过Free(...)函数
            if ( m_LockFree.SetInvalid() == false )
                return;

            // 先调用Clear(...)
            m_ReceiveBuffer.Clear();

            // 取消回调
            if ( m_ServiceHandle != null )
            {
                m_ServiceHandle.EventProcessData -= OnListenerProcessMessageBlock;
                m_ServiceHandle.EventDisconnect -= OnListenerDisconnect;
            }

            // 如果是Listener的则需要释放入内存池中...
            if ( m_Listener != null )
                m_Listener.Free( m_ReceiveBuffer );
        }
        #endregion

        #region zh-CHS 共有方法 | en Public Methods
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseSocket()
        {
            if ( m_ServiceHandle != null )
                m_ServiceHandle.CloseSocket();
            else if ( m_ConnectHandle != null )
                m_ConnectHandle.CloseSocket();
            else
                throw new Exception( "ClientSocketHandler.CloseSocket(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendMessageBlock"></param>
        public void SendTo( MessageBlock sendMessageBlock )
        {
            if ( m_ServiceHandle != null )
                m_ServiceHandle.SendTo( sendMessageBlock );
            else if ( m_ConnectHandle != null )
                m_ConnectHandle.SendTo( sendMessageBlock );
            else
                throw new Exception( "ClientSocketHandler.SendTo(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MessageBlock GetNewSendMessageBlock()
        {
            if ( m_ServiceHandle != null )
                return m_ServiceHandle.Owner.ServiceHandleManager.GetNewSendMessageBlock();
            else if ( m_ConnectHandle != null )
                return m_ConnectHandle.Owner.ConnectHandlerManager.GetNewSendMessageBlock();
            else
                throw new Exception( "ClientSocketHandler.GetNewSendMessageBlock(...) - m_SocketClientAtServer == null || m_SocketClientAtClient == null error!" );
        }
        #endregion

        #region zh-CHS 内部通知信号方法 | en Internal Signal Method
        /// <summary>
        /// 接受信号
        /// </summary>
        internal void ReceiveSignal()
        {
            // 需要检测，可能还没有设置m_NetState，就开始调用处理了
            if ( m_NetState != null )
                m_NetState.OnReceive();
        }

        /// <summary>
        /// 断开信号
        /// </summary>
        internal void DisconnectSignal()
        {
            // 需要检测，可能还没有设置m_NetState，就开始调用处理了
            if ( m_NetState != null )
                m_NetState.OnDisconnect();
        }
        #endregion

        #region zh-CHS 私有的事件处理函数 | en Private Event Handlers
        /// <summary>
        /// 接受到新的数据包
        /// </summary>
        /// <param name="RecvMessageBlock"></param>
        private void OnListenerProcessMessageBlock( object sender, ProcessDataAtServerEventArgs recvMessageBlock )
        {
            // 内部已经有锁定(Listener)
            m_ReceiveBuffer.Enqueue( recvMessageBlock.ProcessData.ReadPointer(), 0, recvMessageBlock.ProcessData.WriteLength );

            ReceiveSignal();
        }

        /// <summary>
        /// 断开
        /// </summary>
        private void OnListenerDisconnect( object sender, DisconnectAtServerEventArgs emptyEventArgs )
        {
            DisconnectSignal();
        }
        #endregion
    }
}
#endregion