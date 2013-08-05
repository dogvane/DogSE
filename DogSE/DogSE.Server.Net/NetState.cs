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
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Demo.Mmose.Core.Common;
using Demo.Mmose.Core.Common.Atom;
using Demo.Mmose.Core.Common.Log;
using Demo.Mmose.Core.Entity.Character;
using Demo.Mmose.Core.Server.Language;
using Demo.Mmose.Core.Timer;
using Demo.Mmose.Core.Util;
using Demo.Mmose.Core.World;
using Demo.Mmose.Net;
using Demo.Mmose.Core.Server;
using DogSE.Library.Time;
using DogSE.Server.Common;

#endregion

namespace DogSE.Server.Net
{
    /// <summary>
    /// 网络的主状态,每个连接都会产生新的NetState
    /// </summary>
    [MultiThreadedSupport("zh-CHS", "当前的类所有成员都可锁定,支持多线程操作")]
    public sealed partial class NetState
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose
        /// <summary>
        /// 有实例产生时调用
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="messagePump"></param>
        internal NetState(ClientSocketManager clientSocket, MessagePump messagePump)
        {
            if (clientSocket == null)
                throw new ArgumentNullException("clientSocket", "NetState.NetState(...) - clientSocket == null error!");

            if (messagePump == null)
                throw new ArgumentNullException("messagePump", "NetState.NetState(...) - messagePump == null error!");

            if (messagePump.World == null)
                throw new ArgumentNullException("messagePump.World", "NetState.NetState(...) - messagePump.World == null error!");

            m_Socket = clientSocket;
            m_MessagePump = messagePump;
            m_ConnectedOn = OneServer.NowTime;
            m_World = messagePump.World;
            m_ToString = clientSocket.Address;
            m_NextCheckActivity = m_ConnectedOn + m_World.CheckAliveTime;

            IPAddress ipAddress = null;
            IPAddress.TryParse(clientSocket.RemoteOnlyIP, out ipAddress);

            if (ipAddress == null)
                m_NetAddress = new IPEndPoint(IPAddress.None, clientSocket.RemotePort);
            else
                m_NetAddress = new IPEndPoint(ipAddress, clientSocket.RemotePort);
        }

        /// <summary>
        /// 无效实例产生时调用
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="messagePump"></param>
        public NetState()
        {
            m_Socket = null;
            m_MessagePump = null;
            m_ConnectedOn = OneServer.NowTime;
            m_World = null;
            m_ToString = "0.0.0.0";
            m_NextCheckActivity = DateTime.MaxValue;

            m_NetAddress = new IPEndPoint(IPAddress.None, 0);

            m_Disposed.SetInvalid();
        }
        #endregion

        #region zh-CHS 共有属性 | en Public Properties

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private Serial m_Serial = Serial.MinusOne;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public Serial Serial
        {
            get { return m_Serial; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 客户开始连接的时间
        /// </summary>
        private DateTime m_ConnectedOn = OneServer.NowTime;
        #endregion
        /// <summary>
        /// 客户开始连接的时间
        /// </summary>
        public DateTime ConnectedOn
        {
            get { return m_ConnectedOn; }
        }

        /// <summary>
        /// 客户总共连接的时间
        /// </summary>
        public TimeSpan ConnectedFor
        {
            get { return (OneServer.NowTime - m_ConnectedOn); }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 客户的地址
        /// </summary>
        private IPEndPoint m_NetAddress;
        #endregion
        /// <summary>
        /// 客户的地址
        /// </summary>
        public IPEndPoint NetAddress
        {
            get { return m_NetAddress; }
        }

        /// <summary>
        /// 加解密的数据包接口
        /// </summary>
        public IPacketEncoder PacketEncoder { get; set; }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 客户是否在运行
        /// </summary>
        private LockCheck m_Running = new LockCheck(false);
        #endregion
        /// <summary>
        /// 客户是否在运行, 如果没有表示已经断开了连接
        /// </summary>
        public bool Running
        {
            get { return m_Running.IsValid(); }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 网络句柄
        /// </summary>
        private ClientSocketManager m_Socket;
        #endregion
        /// <summary>
        /// 客户端连接的句柄
        /// </summary>
        public ClientSocketManager NetSocket
        {
            get { return m_Socket; }
        }

        /// <summary>
        /// 接收到的环形缓冲区
        /// </summary>
        public ReceiveQueue ReceiveBuffer
        {
            get { return m_Socket == null ? null : m_Socket.ReceiveBuffer; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 包处理优先列队
        /// </summary>
        private PacketHandlerManager m_PacketPriorityManager = new PacketHandlerManager();
        #endregion
        /// <summary>
        /// 包处理优先列队
        /// </summary>
        public PacketHandlerManager PacketManager
        {
            get { return m_PacketPriorityManager; }
        }

        /// <summary>
        /// 缓冲区内还有多少没有发送的数据
        /// </summary>
        public long WaitSendSize
        {
            get { return m_SendQueue.WaitSendSize; }
        }

        /// <summary>
        /// 客户端的版本号
        /// </summary>
        public VersionInfo Version { get; set; }

        /// <summary>
        /// 人物
        /// </summary>
        public CharacterBase Player { get; set; }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 是否检查网络活动
        /// </summary>
        private bool m_bCheckAlive = true;
        #endregion
        /// <summary>
        /// 是否检查网络活动
        /// </summary>
        public bool IsCheckActivity
        {
            get { return m_bCheckAlive; }
            set { m_bCheckAlive = value; }
        }

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
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 消息处理的类
        /// </summary>
        private MessagePump m_MessagePump;
        #endregion
        /// <summary>
        /// 消息处理的类
        /// </summary>
        public MessagePump MessagePump
        {
            get { return m_MessagePump; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_SendMaxSize1Sec = NetStateManager.SendMaxSize1Sec;
        #endregion
        /// <summary>
        /// 每秒最大的传输率
        /// </summary>
        public long SendMaxSize1Sec
        {
            get { return m_SendMaxSize1Sec; }
            set { m_SendMaxSize1Sec = value; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_SendCachedMaxSize = NetStateManager.SendCachedMaxSize;
        #endregion
        /// <summary>
        /// 允许最大的传输率堆积率
        /// </summary>
        public long SendCachedMaxSize
        {
            get { return m_SendCachedMaxSize; }
            set { m_SendCachedMaxSize = value; }
        }

        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_ReceiveCachedMaxSize = NetStateManager.ReceiveCachedMaxSize;
        #endregion
        /// <summary>
        /// 允许最大的接收堆积率
        /// </summary>
        public long ReceiveCachedMaxSize
        {
            get { return m_ReceiveCachedMaxSize; }
            set { m_ReceiveCachedMaxSize = value; }
        }
        #endregion

        #region zh-CHS 私有静态属性 | en Private Static Properties
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static readonly NetState s_InvalidNetState = new NetState();
        #endregion
        /// <summary>
        /// 
        /// </summary>
        internal static NetState InvalidNetState
        {
            get { return s_InvalidNetState; }
        }
        #endregion

        #region zh-CHS 内部方法 | en Internal Method
        /// <summary>
        /// 开始运行客户端的处理
        /// </summary>
        internal void Start()
        {
            // 断开处理在设置NetState时候检测
            m_Running.SetValid();
            m_Disposed.SetValid();

            JoinWorld();

            // 连接的通知事件
            EventHandler<NetStateConnectEventArgs> tempEvent = m_EventConnect;
            if (tempEvent != null)
            {
                NetStateConnectEventArgs netStateConnectEventArgs = new NetStateConnectEventArgs(this);
                tempEvent(this, netStateConnectEventArgs);
            }

            // 设置NetState(需位于Start(...)函数之后)，检查是否已断开与已收到新数据。
            m_Socket.NetState = this;
        }

        /// <summary>
        /// 进入World集合,在Start中调用
        /// </summary>
        internal void JoinWorld()
        {
            // 获取唯一的序号
            m_Serial = m_World.NetStateManager.ExclusiveSerial.NextExclusiveSerial();

            // 添加入世界网络状态管理实体
            m_World.NetStateManager.InternalAddNetState(m_Serial, this);
        }

        /// <summary>
        /// 退出World集合,在BaseWorld中调用
        /// </summary>
        internal void ExitWorld()
        {
            // 退出入世界网络状态管理实体
            m_World.NetStateManager.InternalRemoveNetState(m_Serial);

            // 置空
            m_Serial = Serial.MinusOne;
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private long m_SendPackSize1Sec = 0;
        /// <summary>
        /// 
        /// </summary>
        private DateTime m_SendTimeStart = OneServer.NowTime;
        /// <summary>
        /// 
        /// </summary>
        private readonly static TimeSpan s_TimeSpan1Sec = TimeSpan.FromSeconds(1.0);
        #endregion
        /// <summary>
        /// 发送缓存的数据
        /// </summary>
        /// <returns>是否成功处理缓存的发送</returns>
        internal bool Flush()
        {
            if (m_SendQueue.IsEmpty == true)
                return true;

            if (Running == false)
                return false;

            // 等待发送的数据累计过多，断开
            if (m_SendQueue.WaitSendSize >= SendCachedMaxSize)
            {
                // 需要注释
                Logs.Warn("NetState[{0}] Flush(...) - WaitSendSize[{1}] >= {2} warning (发送缓存的数据包过大)!", ToString(), m_SendQueue.WaitSendSize, SendCachedMaxSize);

                Dispose();

                return false;
            }

            if (m_SendPackSize1Sec >= SendMaxSize1Sec)
            {
                DateTime nowDateTime = OneServer.NowTime;

                // 如果小于计算的时间,表示已经超过了数据流量,等待下一个时间
                if (nowDateTime < (m_SendTimeStart + s_TimeSpan1Sec))
                    return true;
                else    // 如果已经超过计算的时间,开始重新计算时间与数据
                {
                    m_SendTimeStart = nowDateTime;
                    m_SendPackSize1Sec = 0;
                }
            }

            SendBuffer sendBuffer = m_SendQueue.Dequeue();
            while (sendBuffer.IsNull == false)
            {
                MessageBlock messageBlock = m_Socket.GetNewSendMessageBlock();
                if (messageBlock == null)
                    throw new ArgumentNullException("messageBlock", "NetState.Flush(...) - messageBlock == null error!");

                if (sendBuffer.Length > messageBlock.Size)
                    throw new ArgumentException("NetState.Flush(...) - sendBuffer.Length > messageBlock.Length error!", "sendBuffer");

                Marshal.Copy(sendBuffer.Buffer, 0, messageBlock.WritePointer(), (int)sendBuffer.Length);

                messageBlock.WritePointer((int)sendBuffer.Length);

                // XG
                ++SanGuo.SanGuoMonitor.tcpMonitor.front.c13.runCount;
                SanGuo.SanGuoMonitor.tcpMonitor.front.c14.runCount += sendBuffer.Length;

                m_Socket.SendTo(messageBlock);

                // 释放数据
                sendBuffer.Release();

                // 累计发送的数据大小
                m_SendPackSize1Sec += sendBuffer.Length;

                // 已经超过了数据流量,就等待下一秒开始重新计算
                if (m_SendPackSize1Sec >= SendMaxSize1Sec)
                    break;

                // 需添加检测是否还有数据 否则以后就发不出去数据了（该问题解决）
                sendBuffer = m_SendQueue.Dequeue();
            }

            return true;
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 检查是否在线的时间段
        /// </summary>
        private DateTime m_NextCheckActivity = OneServer.NowTime;
        #endregion
        /// <summary>
        /// 检查用户是否在线,根据用户传输数据包的时间来判定
        /// </summary>
        /// <returns></returns>
        internal bool CheckAlive()
        {
            if (Running == false)
                return false;

            if (m_bCheckAlive == false || OneServer.NowTime < m_NextCheckActivity)
                return true;

            Dispose();

            Logs.WriteLine(LogMessageType.MSG_NOTICE, LanguageString.SingletonInstance.NetStateString001, this);

            return false;
        }
        #endregion

        #region zh-CHS 共有方法 | en public Method
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 当前需要处理的集合
        /// </summary>
        private ConcurrentQueue<Packet> m_PacketSendQueue_Lowest = new ConcurrentQueue<Packet>();
        /// <summary>
        /// 当前需要处理的集合
        /// </summary>
        private ConcurrentQueue<Packet> m_PacketSendQueue_Normal = new ConcurrentQueue<Packet>();
        /// <summary>
        /// 当前需要处理的集合
        /// </summary>
        private ConcurrentQueue<Packet> m_PacketSendQueue_Highest = new ConcurrentQueue<Packet>();
        /// <summary>
        /// 发送数据的锁
        /// </summary>
        LockInOut m_LockSend = new LockInOut(false);
        #endregion

        /// <summary>
        /// 发送数据(在多线程中主要实现了顺序的发送)
        /// </summary>
        /// <param name="packet">需要发送的数据包</param>
        public void Send(Packet packet)
        {
            if (Running == false)
                return;

            switch (packet.PacketPriority)
            {
                case PacketPriority.Highest:
                case PacketPriority.AboveNormal:

                    m_PacketSendQueue_Highest.Enqueue(packet);
                    break;
                case PacketPriority.Normal:

                    m_PacketSendQueue_Normal.Enqueue(packet);
                    break;
                case PacketPriority.BelowNormal:
                case PacketPriority.Lowest:

                    m_PacketSendQueue_Lowest.Enqueue(packet);
                    break;
                default:

                    m_PacketSendQueue_Normal.Enqueue(packet);
                    break;
            }

            // XG
            SanGuo.TcpMonitor monitor = SanGuo.SanGuoMonitor.tcpMonitor;
            ++monitor.front.c11.runCount; // 消息数量
            monitor.front.c12.runCount += packet.WriterStream.Length; // 消息长度
            
            // 防止发送的顺序出错
            if (m_LockSend.InLock() == false)
                return;

            Packet sendPacket = null;
            do
            {
                sendPacket = null;

                if (m_PacketSendQueue_Highest.TryDequeue(out sendPacket) == false)
                    if (m_PacketSendQueue_Normal.TryDequeue(out sendPacket) == false)
                        if (m_PacketSendQueue_Lowest.TryDequeue(out sendPacket) == false)
                            break;

                if (sendPacket != null)
                    InternalSend(sendPacket);

            } while (sendPacket != null);

            m_LockSend.OutLock();

            // 如果没有错误且需要等待发送，则放在全局的处理中。。。
            if (m_SendQueue.IsEmpty == false)
                m_World.FlushNetStates(this);
        }

        #region zh-CHS 内部SendQueue属性 | en Internal SendQueue Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 发送数据的集合
        /// </summary>
        private SendQueue m_SendQueue = new SendQueue();
        #endregion
        /// <summary>
        /// 发送数据的集合
        /// </summary>
        internal SendQueue SendQueue
        {
            get { return m_SendQueue; }
        }
        #endregion
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="packet">需要发送的数据包</param>
        [MultiThreadedWarning("zh-CHS", "此处只在单一的线程内回调:(不存在多线程问题)警告!")]
        private void InternalSend(Packet packet)
        {
            if (Running == false)
            {
                // 表示已处理发送
                packet.Release();
                return;
            }

            // 连接的通知事件
            EventHandler<NetStateSendPacketEventArgs> tempEvent = m_EventSendPacket;
            if (tempEvent != null)
            {
                NetStateSendPacketEventArgs sendEventArgs = new NetStateSendPacketEventArgs(this, packet);
                tempEvent(this, sendEventArgs);

                if (sendEventArgs.IsCancelSend == true)
                {
                    // 表示已处理发送
                    packet.Release();
                    return;
                }
            }

            long iLength = 0;

            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile((byte)packet.PacketID);
            DateTime dateTimeStart = (packetProfile == null ? DateTime.MinValue : OneServer.NowTime);
            {
                PacketBuffer packetBuffer = packet.AcquireBuffer();
                if (packetBuffer.IsNULL == true)
                    throw new Exception("NetState.InternalSend(...) - packetBuffer.IsNULL == true error!");

                byte[] byteBuffer = packetBuffer.Buffer;
                long lLength = packetBuffer.Length;

                // 加密数据
                if (PacketEncoder != null)
                    PacketEncoder.EncodeOutgoingPacket(this, ref byteBuffer, ref lLength);

                m_SendQueue.Enqueue(byteBuffer, 0, lLength);

                // 表示已处理发送
                packet.Release();

                // 实际压缩后的数据长度
                iLength = lLength;
            }
            if (packetProfile != null)
                packetProfile.Record(iLength, OneServer.NowTime - dateTimeStart);
        }

        #region zh-CHS 私有常量 | en Private Constants
        /// <summary>
        /// 
        /// </summary>
        private readonly static int DISCONNECT_SECOND = 5;
        #endregion
        /// <summary>
        /// 清理当前的网络数据,并断开连接(Flush == true 则默认5秒以后断开)
        /// </summary>
        /// <param name="bFlush"></param>
        public void Dispose()
        {
            Dispose(true, DISCONNECT_SECOND);
        }

        /// <summary>
        /// 清理当前的网络数据,并断开连接(如果 bFlush == true 则默认5秒以后断开)
        /// </summary>
        /// <param name="bFlush"></param>
        public void Dispose(bool bFlush)
        {
            Dispose(bFlush, DISCONNECT_SECOND);
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 是否已经处理过断开
        /// </summary>
        private LockCheck m_Disposed = new LockCheck(true);
        #endregion

        #region zh-CHS 私有 InsideDispose 方法 | en Private InsideDispose Methods
        /// <summary>
        /// 断开连接
        /// </summary>
        private void InsideDispose()
        {
            m_Socket.CloseSocket();
        }
        #endregion
        /// <summary>
        /// 清理当前的网络数据,并断开连接
        /// </summary>
        /// <param name="bFlush"></param>
        public void Dispose(bool bFlush, long waitSeconds)
        {
            // 防止多线程时多次调用Dispose(...)有问题
            if (m_Disposed.SetInvalid() == false)
                return;

            if (bFlush == true)
            {
                Flush();

                TimeSlice.StartTimeSlice(TimeSpan.FromSeconds(waitSeconds), InsideDispose);
            }
            else
                InsideDispose();
        }

        #endregion

        #region zh-CHS 方法覆盖 | en Override Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 当前实例的IP地址字符串
        /// </summary>
        private string m_ToString = string.Empty;
        #endregion
        /// <summary>
        /// 当前实例的IP地址字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_ToString;
        }
        #endregion

        #region zh-CHS 私有的事件处理函数 | en Private Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RecvMessageBlock"></param>
        internal void OnReceive()
        {
            if (Running == false)
                return;

            m_NextCheckActivity = OneServer.NowTime + m_World.CheckAliveTime;

            m_MessagePump.OnReceive(this);
        }

        /// <summary>
        /// 调用ClientSocketHandler.CloseSocket(...)的时候会产生此调用
        /// </summary>
        internal void OnDisconnect()
        {
            // 默认是有效地，只检测有没有已经断开，如果已经断开，则不继续处理
            if (m_Running.SetInvalid() == false)
                return;

            m_Socket.Free();

            // 如果还存在需要发送的数据,则直接清空
            if (m_SendQueue.IsEmpty == false)
                m_SendQueue.Clear(); // 内部已经有锁定

            // 断开的通知事件
            EventHandler<NetStateDisconnectEventArgs> tempEvent = m_EventDisconnect;
            if (tempEvent != null)
            {
                NetStateDisconnectEventArgs netStateDisconnectEventArgs = new NetStateDisconnectEventArgs(this);
                tempEvent(this, netStateDisconnectEventArgs);
            }

            // 放入无效客户端的集合内,等待处理
            m_World.DisposedNetStates(this);

            Logs.WriteLine(LogMessageType.MSG_INFO, LanguageString.SingletonInstance.NetStateString002, this);
        }
        #endregion

        #region zh-CHS 共有事件 | en Public Event
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private EventHandler<NetStateConnectEventArgs> m_EventConnect;
        /// <summary>
        /// 
        /// </summary>
        private SpinLock m_LockEventConnect = new SpinLock();
        #endregion
        /// <summary>
        /// 当网络连接时调用
        /// </summary>
        [MultiThreadedWarning("zh-CHS", "此处会在不同的线程内回调:(多线程)警告!")]
        public event EventHandler<NetStateConnectEventArgs> EventConnect
        {
            add
            {
                SpinLockEx.ReliableEnter(ref m_LockEventConnect);
                try
                {
                    m_EventConnect += value;
                }
                finally
                {
                    m_LockEventConnect.Exit();
                }
            }
            remove
            {
                SpinLockEx.ReliableEnter(ref m_LockEventConnect);
                try
                {
                    m_EventConnect -= value;
                }
                finally
                {
                    m_LockEventConnect.Exit();
                }
            }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private EventHandler<NetStateDisconnectEventArgs> m_EventDisconnect;
        /// <summary>
        /// 
        /// </summary>
        private SpinLock m_LockEventDisconnect = new SpinLock();
        #endregion
        /// <summary>
        /// 当网络断开时调用
        /// </summary>
        [MultiThreadedWarning("zh-CHS", "此处会在不同的线程内回调:(多线程)警告!")]
        public event EventHandler<NetStateDisconnectEventArgs> EventDisconnect
        {
            add
            {
                SpinLockEx.ReliableEnter(ref m_LockEventDisconnect);
                try
                {
                    m_EventDisconnect += value;
                }
                finally
                {
                    m_LockEventDisconnect.Exit();
                }
            }
            remove
            {
                SpinLockEx.ReliableEnter(ref m_LockEventDisconnect);
                try
                {
                    m_EventDisconnect -= value;
                }
                finally
                {
                    m_LockEventDisconnect.Exit();
                }
            }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private EventHandler<NetStateSendPacketEventArgs> m_EventSendPacket;
        /// <summary>
        /// 
        /// </summary>
        private SpinLock m_LockEventSendPacket = new SpinLock();
        #endregion
        /// <summary>
        /// 当网络断开时调用
        /// </summary>
        [MultiThreadedWarning("zh-CHS", "此处会在不同的线程内回调:(多线程)警告!")]
        public event EventHandler<NetStateSendPacketEventArgs> EventSendPacket
        {
            add
            {
                SpinLockEx.ReliableEnter(ref m_LockEventSendPacket);
                try
                {
                    m_EventSendPacket += value;
                }
                finally
                {
                    m_LockEventSendPacket.Exit();
                }
            }
            remove
            {
                SpinLockEx.ReliableEnter(ref m_LockEventSendPacket);
                try
                {
                    m_EventSendPacket -= value;
                }
                finally
                {
                    m_LockEventSendPacket.Exit();
                }
            }
        }
        #endregion

        #region zh-CHS 内部 Process Queue 方法 | en Internal Process Queue Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 表示当前已加入在处理列表中
        /// </summary>
        private LockInOut m_IsInProcessQueue = new LockInOut(false);
        #endregion
        /// <summary>
        /// 表示当前已加入在处理列表中(减少处理列表的长度)
        /// </summary>
        /// <returns>是否成功，检查当前是否已加入在处理列表中</returns>
        internal bool InProcessQueue()
        {
            return m_IsInProcessQueue.InLock();
        }

        /// <summary>
        /// 表示当前已不再处理列表中(减少处理列表的长度)
        /// </summary>
        internal void OutProcessQueue()
        {
            m_IsInProcessQueue.OutLock();
        }
        #endregion

        #region zh-CHS 内部 Flush Queue 方法 | en Internal Flush Queue Methods
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 表示当前已加入在处理列表中 0 == FALSE 1 == TRUE 
        /// </summary>
        private LockInOut m_IsInFlushQueue = new LockInOut(false);
        #endregion
        /// <summary>
        /// 表示当前已加入在处理列表中(减少处理列表的长度)
        /// </summary>
        /// <returns></returns>
        internal bool InFlushQueue()
        {
            return m_IsInFlushQueue.InLock();
        }

        /// <summary>
        /// 表示当前已不再处理列表中(减少处理列表的长度)
        /// </summary>
        internal void OutFlushQueue()
        {
            m_IsInFlushQueue.OutLock();
        }
        #endregion
    }
}
#endregion

