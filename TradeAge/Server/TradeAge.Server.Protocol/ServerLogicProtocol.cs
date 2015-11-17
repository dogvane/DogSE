
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;
using DogSE.Server.Core.LogicModule;

namespace DogSE.Server.Core.Protocol.AutoCode
{
    /// <summary>
    /// 服务器业务逻辑注册管理器
    /// </summary>
    public static class ServerLogicProtoclRegister
    {
        private static readonly List<IProtoclAutoCode> list = new List<IProtoclAutoCode>();

        /// <summary>
        /// 注册所有模块的网络消息到包管理器里
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="handlers"></param>
        public static void Register(ILogicModule[] modules, PacketHandlersBase handlers)
        {
            foreach (var m in modules)
            {
                if (m is TradeAge.Server.Interface.Server.IGame)
                {
                    IProtoclAutoCode pac = new IGameAccess1();
                    list.Add(pac);

                    pac.SetModule(m as TradeAge.Server.Interface.Server.IGame);
                    pac.PacketHandlerManager = handlers;
                    pac.Init();
                }                if (m is TradeAge.Server.Interface.Server.ILogin)
                {
                    IProtoclAutoCode pac = new ILoginAccess2();
                    list.Add(pac);

                    pac.SetModule(m as TradeAge.Server.Interface.Server.ILogin);
                    pac.PacketHandlerManager = handlers;
                    pac.Init();
                }                if (m is TradeAge.Server.Interface.Server.IScene)
                {
                    IProtoclAutoCode pac = new ISceneAccess3();
                    list.Add(pac);

                    pac.SetModule(m as TradeAge.Server.Interface.Server.IScene);
                    pac.PacketHandlerManager = handlers;
                    pac.Init();
                }
            }
        }
    }



    class IGameAccess1:IProtoclAutoCode
    {
        public PacketHandlersBase PacketHandlerManager {get;set;}

        TradeAge.Server.Interface.Server.IGame module;

        public void SetModule(ILogicModule m)
        {
            if (m == null)
                throw new ArgumentNullException("ILogicModule");
            module = (TradeAge.Server.Interface.Server.IGame)m;
            if (module == null)
            {
                throw new NullReferenceException(string.Format("{0} not TradeAge.Server.Interface.Server.IGame", m.GetType().FullName));
            }
        }


        public void Init()
        {
PacketHandlerManager.Register(1, TaskType.Low, Heartbeat);

        }

void Heartbeat(NetState netstate, PacketReader reader){
if (!netstate.IsVerifyLogin) return;
var p1 = reader.ReadInt32();
module.Heartbeat(netstate,p1);
}



    }


    class ILoginAccess2:IProtoclAutoCode
    {
        public PacketHandlersBase PacketHandlerManager {get;set;}

        TradeAge.Server.Interface.Server.ILogin module;

        public void SetModule(ILogicModule m)
        {
            if (m == null)
                throw new ArgumentNullException("ILogicModule");
            module = (TradeAge.Server.Interface.Server.ILogin)m;
            if (module == null)
            {
                throw new NullReferenceException(string.Format("{0} not TradeAge.Server.Interface.Server.ILogin", m.GetType().FullName));
            }
        }


        public void Init()
        {
PacketHandlerManager.Register(1000, OnLoginServer);
PacketHandlerManager.Register(1003, OnCreatePlayer);

        }

void OnLoginServer(NetState netstate, PacketReader reader){
var p1 = reader.ReadUTF8String();
var p2 = reader.ReadUTF8String();
var p3 = reader.ReadInt32();
module.OnLoginServer(netstate,p1,p2,p3);
}
void OnCreatePlayer(NetState netstate, PacketReader reader){
if (!netstate.IsVerifyLogin) return;
var p1 = reader.ReadUTF8String();
var p2 = (TradeAge.Server.Entity.Character.Sex)reader.ReadByte();
module.OnCreatePlayer(netstate,p1,p2);
}



    }


    class ISceneAccess3:IProtoclAutoCode
    {
        public PacketHandlersBase PacketHandlerManager {get;set;}

        TradeAge.Server.Interface.Server.IScene module;

        public void SetModule(ILogicModule m)
        {
            if (m == null)
                throw new ArgumentNullException("ILogicModule");
            module = (TradeAge.Server.Interface.Server.IScene)m;
            if (module == null)
            {
                throw new NullReferenceException(string.Format("{0} not TradeAge.Server.Interface.Server.IScene", m.GetType().FullName));
            }
        }


        public void Init()
        {
PacketHandlerManager.Register(1100, OnMove);

        }

void OnMove(NetState netstate, PacketReader reader){
if (!netstate.IsVerifyLogin) return;
var p1 = new DateTime(reader.ReadLong64());
 var p2 = Vector2ReadProxy.Read(reader);
 var p3 = Vector2ReadProxy.Read(reader);
module.OnMove(netstate,p1,p2,p3);
}




        /// <summary>
        /// 
        /// </summary>
    public class Vector2ReadProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static TradeAge.Server.Entity.Common.Vector2 Read(PacketReader reader)
        {
            TradeAge.Server.Entity.Common.Vector2 ret = new TradeAge.Server.Entity.Common.Vector2();

ret.X = reader.ReadFloat();
ret.Y = reader.ReadFloat();


            return ret;
        }
    }

    }

}

